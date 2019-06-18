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
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectTile(int x, int y) {

        selectedTile = tiles[x, y];
        Debug.Log("selected tile = " + selectedTile.position.ToString());
    }
}
