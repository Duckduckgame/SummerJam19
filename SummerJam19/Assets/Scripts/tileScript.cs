using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    int mapSizeX = 10;
    int mapSizeY = 10;

    public GameObject tileGO;
    public tileInfo[,] tiles;

    public tileInfo selectedTile;

    public enum growthDirection {up, upRight, right, downRight, down, downLeft, left, upLeft }

    #region models
    public GameObject flowerBall;

    public GameObject straightVine;
    public GameObject diagVine;

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

        tiles[3, 3].crntType = tileInfo.tileType.Vine;

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
            selectionPlane = Instantiate(selectionPlane, new Vector3(x, 0.2f, y), Quaternion.identity, this.transform);
        }
        selectionPlane.transform.position = new Vector3(x, 0.2f, y);
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

            tileInfo from = vineDirection(x, y);

            growthDirection chosenDir = findGrowthDirection(from, target);

            int vineRot = vineRotation(chosenDir);

            Instantiate(straightVine, from.position, Quaternion.Euler(0, vineRot, 0), target.transform);
            target.crntType = tileInfo.tileType.Vine;
        }
    }

    public growthDirection findGrowthDirection(tileInfo from, tileInfo to) {
        int fromX = Mathf.RoundToInt(from.position.x);
        int fromY = Mathf.RoundToInt(from.position.y);

        int toX = Mathf.RoundToInt(to.position.x);
        int toY = Mathf.RoundToInt(to.position.y);

        //is above
        if (toY > fromY) {
            
            //if (toX > fromX) { return growthDirection.upRight; }
            if (toX == fromX) { return growthDirection.up; }
           // if (toX < fromX) { return growthDirection.upLeft;  }
        }
        //is same level
        if (toY == fromY) {
            if (toX > fromX) { return growthDirection.right; }
            if (toX == fromX) { Debug.LogError("tile 'from' is same as 'to'"); }
            if (toX < fromX) { return growthDirection.left; }
        }
        //is below
        if (toY < fromY) {
           // if (toX > fromX) { return growthDirection.downRight; }
            if (toX == fromX) { return growthDirection.down; }
           // if (toX < fromX) { return growthDirection.downLeft; }
        }
        Debug.LogError("no growth direction found");
        return growthDirection.up;
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
            return false;
        if (tiles[x + 1, y].crntType == tileInfo.tileType.Vine || tiles[x - 1, y].crntType == tileInfo.tileType.Vine || tiles[x, y + 1].crntType == tileInfo.tileType.Vine || tiles[x, y - 1].crntType == tileInfo.tileType.Vine/* || tiles[x + 1, y + 1].crntType == tileInfo.tileType.Vine || tiles[x - 1, y - 1].crntType == tileInfo.tileType.Vine || tiles[x + 1, y - 1].crntType == tileInfo.tileType.Vine || tiles[x - 1, y + 1].crntType == tileInfo.tileType.Vine*/) {
            return true;
        }
        return false;
    }

    tileInfo vineDirection(int x, int y) {
        tileInfo selectedDir = tiles[0,0];
        List<tileInfo> possibleDirections = new List<tileInfo>();

        if (tiles[x + 1, y].crntType == tileInfo.tileType.Vine)
            possibleDirections.Add(tiles[x + 1, y]);

        if (tiles[x - 1, y].crntType == tileInfo.tileType.Vine)
            possibleDirections.Add(tiles[x - 1, y]);

        if (tiles[x, y + 1].crntType == tileInfo.tileType.Vine)
            possibleDirections.Add(tiles[x, y + 1]);

        if (tiles[x, y -1 ].crntType == tileInfo.tileType.Vine)
            possibleDirections.Add(tiles[x + 1, y -1]);

        selectedDir = possibleDirections[Random.Range(possibleDirections.Count -1, possibleDirections.Count)];
        return selectedDir;
    }
}
