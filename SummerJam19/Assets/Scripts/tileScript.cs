using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    int mapSizeX = 50;
    int mapSizeY = 50;

    public GameObject tileGO;
    public tileInfo[,] tiles;

    public tileInfo selectedTile;

    public enum growthDirection { up, upRight, right, downRight, down, downLeft, left, upLeft }

    #region models
    public GameObject flowerBall;

    public GameObject[] straightVine;
    

    #endregion


    public GameObject selectionPlane;
    

    // Start is called before the first frame update
    void Start()
    {
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
               
            }

        }

        tiles[15, 15].crntType = tileInfo.tileType.Vine;
        Instantiate(flowerBall, tiles[15, 15].position, Quaternion.identity);
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


        //Debug.Log("selected tile = " + selectedTile.position.ToString());
        Debug.Log("tile type = " + selectedTile.crntType);
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

            List<tileInfo> from = vineDirection(x, y);

            foreach (tileInfo TI in from)
            {
                growthDirection chosenDir = findGrowthDirection(TI, target);

                int vineRot = vineRotation(chosenDir);
                Vector3 pos = target.position;
                pos.y = Random.Range(-0.02f, 0.05f);

                Instantiate(straightVine[Random.Range(0,straightVine.Length)], pos, Quaternion.Euler(0, vineRot, 0), target.transform);
                target.crntType = tileInfo.tileType.Vine;
            }

           /* growthDirection chosenDir = findGrowthDirection(from, target);

            int vineRot = vineRotation(chosenDir);

            Instantiate(straightVine, target.position, Quaternion.Euler(0, vineRot, 0), target.transform);
            target.crntType = tileInfo.tileType.Vine;*/
        }
    }

    public growthDirection findGrowthDirection(tileInfo from, tileInfo to) {
        int fromX = Mathf.RoundToInt(from.position.x);
        int fromY = Mathf.RoundToInt(from.position.z);

        int toX = Mathf.RoundToInt(to.position.x);
        int toY = Mathf.RoundToInt(to.position.z);

        //is above
        if (toY > fromY) {
            
            //if (toX > fromX) { return growthDirection.upRight; }
            if (toX == fromX) { return growthDirection.down; }
           // if (toX < fromX) { return growthDirection.upLeft;  }
        }
        //is same level
        if (toY == fromY) {
            if (toX > fromX) { return growthDirection.left; }
            if (toX == fromX) { Debug.LogError("tile 'from' is same as 'to'"); }
            if (toX < fromX) { return growthDirection.right; }
        }
        //is below
        if (toY < fromY) {
           // if (toX > fromX) { return growthDirection.downRight; }
            if (toX == fromX) { return growthDirection.up; }
           // if (toX < fromX) { return growthDirection.downLeft; }
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
        if (tiles[x, y].crntType == tileInfo.tileType.Vine || tiles[x, y].crntType == tileInfo.tileType.Flower)
        {
            Debug.Log("Tile not Valid");
            return false;
        }
        if (tiles[x + 1, y].crntType == tileInfo.tileType.Vine || tiles[x - 1, y].crntType == tileInfo.tileType.Vine || tiles[x, y + 1].crntType == tileInfo.tileType.Vine || tiles[x, y - 1].crntType == tileInfo.tileType.Vine/* || tiles[x + 1, y + 1].crntType == tileInfo.tileType.Vine || tiles[x - 1, y - 1].crntType == tileInfo.tileType.Vine || tiles[x + 1, y - 1].crntType == tileInfo.tileType.Vine || tiles[x - 1, y + 1].crntType == tileInfo.tileType.Vine*/) {
            return true;
        }
        Debug.Log("Tile not Valid");
        return false;
    }

    List<tileInfo> vineDirection(int x, int y) {
        
        List<tileInfo> possibleDirections = new List<tileInfo>();

        if (tiles[x + 1, y].crntType == tileInfo.tileType.Vine)
            possibleDirections.Add(tiles[x + 1, y]);

        if (tiles[x - 1, y].crntType == tileInfo.tileType.Vine)
            possibleDirections.Add(tiles[x - 1, y]);

        if (tiles[x, y + 1].crntType == tileInfo.tileType.Vine)
            possibleDirections.Add(tiles[x, y + 1]);

        if (tiles[x, y -1 ].crntType == tileInfo.tileType.Vine)
            possibleDirections.Add(tiles[x + 1, y -1]);

        //selectedDir = possibleDirections[Random.Range(possibleDirections.Count -1, possibleDirections.Count)];
        return possibleDirections;
    }
}
