using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cameraScript : MonoBehaviour
{

    Plane groundPlane;

    tileScript TS;
    UIManager UIM;

    int selectX = 0;
    int selectY = 0;

    // Start is called before the first frame update
    void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        TS = GameObject.Find("TileManager").GetComponent<tileScript>();
        UIM = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                float distance;
                if (groundPlane.Raycast(ray, out distance))
                {

                    Vector3 worldPosition = ray.GetPoint(distance);
                    selectX = Mathf.RoundToInt(worldPosition.x);
                    selectY = Mathf.RoundToInt(worldPosition.z);

                    Debug.DrawLine(Camera.main.transform.position, worldPosition);
                    //Debug.LogFormat("Clicked Positions: {0} | {1}", selectX, selectY);

                    TS.selectTile(selectX, selectY);
                    //this.transform.parent.localPosition = new Vector3(worldPosition.x, transform.parent.position.y, worldPosition.z);
                }

                if (Physics.Raycast(ray, out hit)){
                    if (hit.point.y > 1f && hit.collider.gameObject.tag == "building") {
                        TS.selectedBuilding = hit.collider.gameObject;
                        UIM.crntType = UIManager.UIType.TriggerPlace;
                    }
                }
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
