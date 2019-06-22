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

    Quaternion targetRot;
    Quaternion oldRot;

    // Start is called before the first frame update
    void Start()
    {
        oldY = transform.position.y;
        target = GameObject.Find("camTarget").GetComponent<Transform>();
        Debug.Log(target.name);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        oldRot = transform.rotation;

        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        targetPos = new Vector3(horiInput, 0, vertInput);

        scrollPos = new Vector3(0, Input.mouseScrollDelta.y * 1.5f, 0);

        //cam.transform.Translate(scrollPos * Time.deltaTime * interpolation, Space.World);

        //if (cam.transform.position.y <= 50f)
        //{
            cam.GetComponent<Rigidbody>().AddForce(scrollPos * 20, ForceMode.Impulse);
            cam.GetComponent<Rigidbody>().velocity = Vector3.zero;
       // }

        target.transform.Translate(targetPos * Time.deltaTime * interpolation);

        if (Input.GetMouseButton(2)) {
            target.transform.eulerAngles += new Vector3(0, Input.GetAxisRaw("Mouse X"), 0);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {

            //target.transform.Rotate(new Vector3(0, 90, 0));
            targetRot = Quaternion.Euler(0, transform.rotation.y + 90, 0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            //target.transform.Rotate(new Vector3(0, -90, 0));
            targetRot = Quaternion.Euler(0, transform.rotation.y - 90, 0);
        }

        //Quaternion.Lerp(oldRot, targetRot, Time.deltaTime * camRotSpeed);
        transform.LookAt(target.transform, Vector3.up);

        if (transform.position.y > 50) {
            cam.orthographic = true;
            cam.orthographicSize = 15;
        }
        if (transform.position.y < 50) {
            cam.orthographic = false;
        }


    }
}
