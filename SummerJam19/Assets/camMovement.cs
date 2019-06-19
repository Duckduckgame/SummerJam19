using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMovement : MonoBehaviour
{

    public float camMvSpeed;
    public float camRotSpeed;
    public float interpolation;

    Vector3 targetPos;
    Vector3 scrollPos;

    float oldY;
    float newY;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        oldY = transform.position.y;

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        targetPos = new Vector3(horiInput, 0, vertInput);

        scrollPos = new Vector3(0, Input.mouseScrollDelta.y * 1.5f, 0);

        //cam.transform.Translate(scrollPos * Time.deltaTime * interpolation, Space.World);

        cam.GetComponent<Rigidbody>().AddForce(scrollPos * 20, ForceMode.Impulse);
        cam.GetComponent<Rigidbody>().velocity = Vector3.zero;


        transform.Translate(targetPos * Time.deltaTime * interpolation);

        if (Input.GetMouseButton(2)) {
            transform.eulerAngles += new Vector3(0, Input.GetAxisRaw("Mouse X"), 0);
        }

        
  
    }
}
