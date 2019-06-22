using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    tickerController TC;
    int mapSizeX = 200;
    int mapSizeY = 200;

    public GameObject tileGO;
    public tileInfo[,] tiles;

    public tileInfo selectedTile;

    public enum growthDirection { up, upRight, right, downRight, down, downLeft, left, upLeft }

    public GameObject flowerBall;

    public GameObject[] straightVine;

    public List<tileInfo> vineTiles;

    public GameObject selectedBuilding;

    public AudioClip[] SFX;

    List<tileInfo> flowerLocations;

    public GameObject selectionPlane;
    public GameObject flowerLocationPlane;
    public GameObject radioPlane;
    public Dictionary<tileInfo, GameObject> maptoRadioTiles = new Dictionary<tileInfo, GameObject>();

    List<GameObject> flowerLocationPlanes;

    public Dictionary<tileInfo, flowerInfo> mapToFlower = new Dictionary<tileInfo, flowerInfo>();

    UIManager UIM;
    flowerController FC;

    public GameObject debugBall;
    

    // Start is called before the first frame update
    void Start()
    {
        vineTiles = new List<tileInfo>();
        flowerLocationPlanes = new List<GameObject>();
        updateFlowerPlacement();

        UIM = GameObject.Find("UIManager").GetComponent<UIManager>();
        TC = GameObject.Find("TickerController").GetComponent<tickerController>();
        FC = GameObject.Find("TickerController").GetComponent<flowerController>();
        generateTiles();

        updateTiles();

        tiles[100, 95].crntType = tileInfo.tileType.Flower;
        GameObject flowerGO1 = Instantiate(flowerBall, tiles[100, 95].position, Quaternion.identity);
        mapToFlower.Add(tiles[100, 95], flowerGO1.GetComponent<flowerInfo>());
        TC.flowers.Add(flowerGO1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            changeTileType();
        }

    }

    public void selectTile(int x, int y) {

        if (selectedTile == null) {
            selectionPlane = Instantiate(selectionPlane, new Vector3(x, 0.01f, y), Quaternion.identity, this.transform);
        }
       
        selectionPlane.transform.position = new Vector3(x, 0.01f, y);
        selectedTile = tiles[x, y];

        if (flowerLocations.Contains(selectedTile)) {
            UIM.crntType = UIManager.UIType.FlowerPlace;
        }
        if (!flowerLocations.Contains(selectedTile)) {
            UIM.crntType = UIManager.UIType.None;
        }
        if (selectedTile.crntType == tileInfo.tileType.Flower)
        {
            if (mapToFlower[selectedTile].crntUpgradeLvl != flowerInfo.upgradeLevel.three) { 
            UIM.crntType = UIManager.UIType.FlowerUpgrade;
            }
        }

       
        Debug.Log("tile type = " + selectedTile.crntType + " at: " + x + "/" + y);
    }

    public void changeTileType() {

        if (selectedTile != null)
        {
            selectedTile.crntType = tileInfo.tileType.Vine;
        }
    }


    public void placeVine(int x, int y) {
        tileInfo target = tiles[x, y];
        
        if (checkVineViability(x, y) == true) {
            Debug.Log("spawn triggered");
            List<tileInfo> from = vineDirection(x, y);

            foreach (tileInfo TI in from)
            {
                growthDirection chosenDir = findGrowthDirection(TI, target);

                int vineRot = vineRotation(chosenDir);
                Vector3 pos = target.position;
                pos.y = Random.Range(-0.02f, 0.05f);

                Instantiate(straightVine[Random.Range(0,straightVine.Length)], pos, Quaternion.Euler(0, vineRot, 0), target.transform);
                target.crntType = tileInfo.tileType.Vine;
                vineTiles.Add(target);
                updateFlowerPlacement();
                chargeForVine();
            }
        }
    }

    public growthDirection findGrowthDirection(tileInfo from, tileInfo to) {
        int fromX = Mathf.RoundToInt(from.position.x);
        int fromY = Mathf.RoundToInt(from.position.z);

        int toX = Mathf.RoundToInt(to.position.x);
        int toY = Mathf.RoundToInt(to.position.z);

        //is above
        if (toY > fromY) {         
            if (toX == fromX) { return growthDirection.down; }
        }
        //is same level
        if (toY == fromY) {
            if (toX > fromX) { return growthDirection.left; }
            if (toX == fromX) { Debug.LogError("tile 'from' is same as 'to'"); }
            if (toX < fromX) { return growthDirection.right; }
        }
        //is below
        if (toY < fromY) {
            if (toX == fromX) { return growthDirection.up; }
        }
        Debug.LogError("no growth direction found");
        return growthDirection.down;
    }

    int vineRotation(growthDirection dir)
    {
        int rot = 0;

        if (dir == growthDirection.up)
            rot = 0;

        if (dir == growthDirection.right)
            rot = 90;

        if (dir == growthDirection.down)
            rot = 180;

        if (dir == growthDirection.left)
            rot = -90;

        return rot;
    }

    bool checkVineViability(int x, int y) {
        if (tiles[x, y].crntType != tileInfo.tileType.Empty)
        {
            Debug.Log("Tile not Valid, crntTile is not empty");
            return false;
        }
            tileInfo[] tilesToCheck = findNeighbours(tiles[x, y], 1, false);

            foreach (tileInfo tile in tilesToCheck) {
                //Instantiate(selectionPlane, tile.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            
                if (tile.crntType == tileInfo.tileType.Vine || tile.crntType == tileInfo.tileType.Flower) {
                    return true;
                
                }
            
            }
        Debug.Log("Tile not Valid, no Neighbours found");
        return false;
    }

    List<tileInfo> findFlowerLocations() {

        List<tileInfo> flowerPlacementTiles = new List<tileInfo>();

        //find all vines
        foreach (tileInfo tile in vineTiles) {
            tileInfo[] neighbourTiles = findNeighbours(tile, 1, true, true);
            bool isClean = true;
            //check every vines neighbour
            foreach (tileInfo neighbour in neighbourTiles) {
                if (neighbour.crntType != tileInfo.tileType.Vine && neighbour.crntType != tileInfo.tileType.Empty && neighbour.crntType != tileInfo.tileType.Water && neighbour.crntType != tileInfo.tileType.RadioActive) {
                    isClean = false;
                }
            }
            //if all neighbours are clean, vine is valid
            if (isClean == true) {
                Debug.Log("clean tile found");
                flowerPlacementTiles.Add(tile);
               
                
            }
            
        }
        return flowerPlacementTiles;
        
    }

    List<tileInfo> vineDirection(int x, int y) {
        
        List<tileInfo> possibleDirections = new List<tileInfo>();
        tileInfo[] foundNeighbours = findNeighbours(tiles[x, y], 1, false);

            foreach (tileInfo tile in foundNeighbours) {
            if (tile.crntType == tileInfo.tileType.Vine || tile.crntType == tileInfo.tileType.Flower) {
                possibleDirections.Add(tile);
            }
        }
         
        return possibleDirections;
    }

    void generateTiles() {

        tiles = new tileInfo[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Vector3 pos = new Vector3(x, 0, y);
                GameObject tileEmpty = Instantiate(tileGO, pos, Quaternion.identity, this.gameObject.transform);
                tileEmpty.transform.name = pos.ToString();
                tiles[x, y] = tileEmpty.GetComponent<tileInfo>();
                tiles[x, y].position = pos;
                tiles[x,y].Xpos = Mathf.RoundToInt(pos.x);
                tiles[x,y].Ypos = Mathf.RoundToInt(pos.z);

            }

        }
    }

    void updateTiles() {

        foreach (tileInfo tile in tiles)
        {

            RaycastHit hit;

            if (Physics.Raycast(tile.position + new Vector3(0, 100, 0), Vector3.down, out hit)){

                if (hit.distance < 99.9f)
                {
                    if (hit.collider.gameObject.tag != "nutrient" && hit.collider.gameObject.tag != "radioactive")
                        tile.crntType = tileInfo.tileType.Full;

                    if (hit.collider.gameObject.tag == "nutrient")
                        tile.hasNutrient = true;


                    if (hit.collider.gameObject.tag == "radioactive")
                    {
                        GameObject radioTile = Instantiate(radioPlane, tile.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                        tile.crntType = tileInfo.tileType.RadioActive;
                        maptoRadioTiles.Add(tile, radioTile);
                        tile.hasRadio = true;
                    }

                }

                if (hit.collider.gameObject.tag == "water") {

                    tile.hasWater = true;
                    tile.crntType = tileInfo.tileType.Water;

                }
                    
            }
            }
    }

    tileInfo[] findNeighbours(tileInfo crntTile, int radius, bool includeDiagonals, bool includeSelf = false) {
        List<tileInfo> targetTiles = new List<tileInfo>();
        if (includeSelf)
            targetTiles.Add(crntTile);

        int x = crntTile.Xpos;
        int y = crntTile.Ypos;
        for (int i = 0; i < radius ; i++) {
            int n = i + 1;
            targetTiles.Add(tiles[x + n, y]);
            targetTiles.Add(tiles[x - n, y]);
            targetTiles.Add(tiles[x, y + n]);
            targetTiles.Add(tiles[x, y - n]);

            if (includeDiagonals == true) {
                targetTiles.Add(tiles[x + n, y + n]);
                targetTiles.Add(tiles[x - n, y - n]);
                targetTiles.Add(tiles[x + n, y - n]);
                targetTiles.Add(tiles[x - n, y + n]);
            }
        }

        
        return targetTiles.ToArray();
    }

    void debugRadius(tileInfo[] targetTiles) {
        foreach (tileInfo tile in targetTiles) {
            Instantiate(selectionPlane, tile.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }

    public void updateFlowerPlacement() {

        flowerLocations = findFlowerLocations();

        GameObject[] GOarray = flowerLocationPlanes.ToArray();

        for (int i = 0; i < GOarray.Length; i++) { 
            Destroy(GOarray[i]);       
        }

        flowerLocationPlanes.Clear();

        foreach (tileInfo tile in flowerLocations) {
            GameObject flowerPlaneGO = Instantiate(flowerLocationPlane, tile.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
            flowerLocationPlanes.Add(flowerPlaneGO);
        }
    }

    public void placeFlower(flowerController.flowerType flowerType) {
        chargeForFlower(FC.flowerTypes[flowerType].GetComponent<flowerInfo>());

        tileInfo[] neighbours = findNeighbours(selectedTile, 1, true, true);
        List<tileInfo> radioTiles = new List<tileInfo>();

        GameObject FGO = Instantiate(FC.flowerTypes[flowerType], selectedTile.position, Quaternion.Euler(0, Random.Range(1, 5) * 45, 0), selectedTile.transform);
        mapToFlower.Add(selectedTile, FGO.GetComponent<flowerInfo>());
        TC.flowers.Add(FGO);

        selectedTile.crntType = tileInfo.tileType.Flower;

        foreach (tileInfo tile in neighbours) {
            if (tile.hasWater == true)
                FGO.GetComponent<flowerInfo>().localWaterAmount++;
            if (tile.hasNutrient == true)
                FGO.GetComponent<flowerInfo>().localNutrientAmount++;
            if (tile.hasRadio)
                radioTiles.Add(tile);
        }

        selectTile(selectedTile.Xpos, selectedTile.Ypos);
        updateFlowerPlacement();

        if (flowerType == flowerController.flowerType.RadioActive) {

            List<tileInfo> finRadioTiles = new List<tileInfo>();
            tileInfo[] radioTiles2 = new tileInfo[8];
            tileInfo[] radioTiles3 = new tileInfo[8];

            foreach (tileInfo tile in radioTiles)
            {
                radioTiles2 = findNeighbours(tile, 1, true, false);

                foreach (tileInfo tile2 in radioTiles2)
                {
                    radioTiles3 = findNeighbours(tile2, 1, true, false);
                }

            }

            for(int i = 0; i < radioTiles.Count; i++) {
                finRadioTiles.Add(radioTiles[i]);
            }
            for (int i = 0; i < radioTiles2.Length; i++)
            {
                if (!finRadioTiles.Contains(radioTiles2[i])) {
                    if (radioTiles2[i].hasRadio) {
                        finRadioTiles.Add(radioTiles2[i]);
                    }
                }
            }
            for (int i = 0; i < radioTiles3.Length; i++)
            {
                if (!finRadioTiles.Contains(radioTiles3[i]))
                {
                    if (radioTiles3[i].hasRadio)
                    {
                        finRadioTiles.Add(radioTiles3[i]);
                    }
                }
            }

            StartCoroutine(FadeRadioActive(finRadioTiles));
        }
    }

    IEnumerator FadeRadioActive(List<tileInfo> radios) {

        foreach (tileInfo tile in radios)
        {

            tile.hasRadio = false;
            tile.crntType = tileInfo.tileType.Empty;
            maptoRadioTiles[tile].GetComponent<Renderer>().material.SetFloat("Vector1_6951F3E2", 1);
                
            

        }

        yield return null;
    }

    public void placeTrigger() {

        GameObject newBuilding  = selectedBuilding.GetComponent<buildingManager>().upgradeBuilding;
        Instantiate(newBuilding, selectedBuilding.transform.position, selectedBuilding.transform.rotation, selectedBuilding.transform);
        selectedBuilding.GetComponent<Renderer>().enabled = false;
        selectedBuilding = null;
        UIM.crntType = UIManager.UIType.None;
    }

    public void chargeForFlower(flowerInfo flower) {

        TC.sunAmount -= flower.baseSunCost;
        TC.nutrientAmount -= flower.baseNutrientCost;
        TC.waterAmount -= flower.baseWaterCost;
    }

    public void chargeForVine() {
        TC.sunAmount--;
    }

    }


