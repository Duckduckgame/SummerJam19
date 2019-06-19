using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMovement : MonoBehaviour
{

    public float camMvSpeed;
    public float camRotSpeed;
    public float interpolation;
    Vector3 targetPos;

    float oldY;
    float newY;

    // Start is called before the first frame update
    void Start()
    {
        oldY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        /* if (Input.GetKey(KeyCode.W))
         {
             transform.localPosition += new Vector3(0, 0, 1);
         }*/

        targetPos = transform.localPosition += new Vector3(horiInput, Input.mouseScrollDelta.y * 1.5f, vertInput);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * interpolation);
  
    }
}
