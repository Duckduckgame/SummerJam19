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


    List<tileInfo> flowerLocations;

    public GameObject selectionPlane;
    public GameObject flowerLocationPlane;
    List<GameObject> flowerLocationPlanes;

    UIManager UIM;

    public GameObject debugBall;
    

    // Start is called before the first frame update
    void Start()
    {
        vineTiles = new List<tileInfo>();
        flowerLocationPlanes = new List<GameObject>();
        updateFlowerPlacement();

        UIM = GameObject.Find("UIManager").GetComponent<UIManager>();
        TC = GameObject.Find("TickerController").GetComponent<tickerController>();

        generateTiles();

        updateTiles();

        tiles[16, 15].crntType = tileInfo.tileType.Flower;
        GameObject flowerGO1 = Instantiate(flowerBall, tiles[16, 15].position, Quaternion.identity);
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
            Debug.Log("Flower tile selected! Press Space to place a Sunflower");
            UIM.crntType = UIManager.UIType.FlowerPlace;
        }
        if (!flowerLocations.Contains(selectedTile)) {
            UIM.crntType = UIManager.UIType.None;
        }
        if (selectedTile.crntType == tileInfo.tileType.Flower) {
            UIM.crntType = UIManager.UIType.FlowerUpgrade;
        }

        //Debug.Log("selected tile = " + selectedTile.position.ToString());
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
                if (neighbour.crntType != tileInfo.tileType.Vine && neighbour.crntType != tileInfo.tileType.Empty) {
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
                   
                    tile.crntType = tileInfo.tileType.Full;
                }
                /*if (hit.distance == 100) {
                    Instantiate(debugBall, hit.point, Quaternion.identity);
                }*/
                if (hit.distance > 100.1f) {

                    tile.crntType = tileInfo.tileType.Full;

                }
                    
            }

            /*if (Physics.Raycast(tile.position, Vector3.up, out hit, 3))
            {
                if (hit.collider != null)
                {
                    GameObject targetGO = hit.collider.gameObject;

                    if (targetGO.tag == "NV") {
                        tile.crntType = tileInfo.tileType.Full;
                        continue;
                    }
                    if (targetGO.tag == "water")
                    {
                        tile.crntType = tileInfo.tileType.Full;
                        tile.hasWater = true;
                        continue;
                    }
                    if (targetGO.tag == "nutrient")
                    {
                        tile.crntType = tileInfo.tileType.Full;
                        tile.hasNutrient = true;
                        continue;
                    }
                    if (targetGO.tag == "radioactive")
                    {
                        tile.crntType = tileInfo.tileType.Full;
                        tile.hasRadio = true;
                        continue;
                    }

                }*/
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

    public void placeFlower() {
        Instantiate(flowerBall, selectedTile.position, Quaternion.identity, selectedTile.transform);
        selectedTile.crntType = tileInfo.tileType.Flower;
        updateFlowerPlacement();
    }

    }


