using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{

    Plane groundPlane;

    tileScript TS;

    int selectX = 0;
    int selectY = 0;

    // Start is called before the first frame update
    void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        TS = GameObject.Find("TileManager").GetComponent<tileScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;
            if (groundPlane.Raycast(ray, out distance)) {

                Vector3 worldPosition = ray.GetPoint(distance);
                selectX = Mathf.RoundToInt(worldPosition.x);
                selectY = Mathf.RoundToInt(worldPosition.z);

                Debug.DrawLine(Camera.main.transform.position, worldPosition);
                //Debug.LogFormat("Clicked Positions: {0} | {1}", selectX, selectY);

                TS.selectTile(selectX, selectY);
            }
        }

        if (Input.GetMouseButton(1))
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {

                Vector3 worldPosition = ray.GetPoint(distance);
                selectX = Mathf.RoundToInt(worldPosition.x);
                selectY = Mathf.RoundToInt(worldPosition.z);

                Debug.DrawLine(Camera.main.transform.position, worldPosition);
                //Debug.LogFormat("Clicked Positions: {0} | {1}", selectX, selectY);

                TS.placeVine(selectX, selectY);
            }
        }
    }
}
