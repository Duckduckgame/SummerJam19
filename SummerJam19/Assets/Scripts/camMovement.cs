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

    Transform target;

    Renderer tilePlainRend;
    // Start is called before the first frame update
    void Start()
    {
        oldY = transform.position.y;
        target = GameObject.Find("camTarget").GetComponent<Transform>();
        Debug.Log(target.name);
        cam = Camera.main;
        tilePlainRend = GameObject.Find("tilePlain").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {


        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        targetPos = new Vector3(horiInput, 0, vertInput);

        scrollPos = new Vector3(0, Input.mouseScrollDelta.y * 1.5f, 0);

        //cam.transform.Translate(scrollPos * Time.deltaTime * interpolation, Space.World);

        //if (cam.transform.position.y <= 50f)
        //{
            cam.GetComponent<Rigidbody>().AddForce(scrollPos * 20, ForceMode.Impulse);
            cam.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (cam.transform.position.y < 0.35f) {
            cam.transform.position = new Vector3(cam.transform.position.x, 0.36f, cam.transform.position.z);
        }
       // }

        target.transform.Translate(targetPos * Time.deltaTime * interpolation);

        if (Input.GetMouseButton(2)) {
            target.transform.eulerAngles += new Vector3(0, Input.GetAxisRaw("Mouse X"), 0);
        }

        

        //Quaternion.Lerp(oldRot, targetRot, Time.deltaTime * camRotSpeed);
        transform.LookAt(target.transform, Vector3.up);

        if (transform.position.y > 50) {
            cam.orthographic = true;
            cam.orthographicSize = 15;
            tilePlainRend.enabled = true;
        }
        if (transform.position.y < 50) {
            cam.orthographic = false;
            tilePlainRend.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 tempPos = cam.transform.position;
            if (cam.transform.position.y < 50) 
                tempPos.y = 51;
            if (cam.transform.position.y > 50)
                tempPos.y = 20;
            cam.transform.position = tempPos;
        }


    }
}
