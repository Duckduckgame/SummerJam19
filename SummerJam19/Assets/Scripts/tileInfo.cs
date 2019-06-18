using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileInfo : MonoBehaviour
{
    public Vector3 position;


    public enum tileType {Empty, Vine, Flower};

    public tileType crntType;

    private void Start()
    {
       
        crntType = tileType.Empty;
    }
}
