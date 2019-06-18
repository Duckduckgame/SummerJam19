using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileInfo : MonoBehaviour
{
    public Vector3 position;

    public int Xpos;
    public int Ypos;

    tileScript TS;

    public enum tileType {Empty, Vine, Flower};

    public tileType crntType;

    tileType oldType = tileType.Empty;

    private void Start()
    {
       TS = GameObject.Find("TileManager").GetComponent<tileScript>();
        Xpos = Mathf.RoundToInt(position.x);
        Ypos = Mathf.RoundToInt(position.y);
        //crntType = tileType.Empty;
    }

    private void Update()
    {
        if (crntType != oldType) {
            updateTileType(crntType);
            oldType = crntType;
        }
    }


    void updateTileType(tileType newType) {

        //Check if this tile has children and if so delete the lot of them
       /* if (this.transform.childCount != 0) {
            Transform[] children = GetComponentsInChildren<Transform>();
            foreach (Transform t in children) {
                Destroy(t.gameObject);
            }
        }

        //create the new children according to tileType
        Instantiate(TS.flowerBall, position, Quaternion.identity, this.transform);*/
    }

}
