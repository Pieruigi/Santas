using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFree : MonoBehaviour
{
    float rotX;
    float rotY;
    float minRotX = -30;
    float maxRotX = 30;
    float mouseSens = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FreeLook();
    }

    void FreeLook()
    {
        //float xDelta = -Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        //float yDelta = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        //Vector3 eulers = transform.eulerAngles + new Vector3(xDelta, yDelta, 0);
        //Debug.Log("Eulers " + eulers);
        ////eulers.x = Mathf.Clamp(eulers.x, minRotX, maxRotX); ;
        //transform.eulerAngles = eulers;

        // Check input
        rotX -= Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;

        // Clamp angles
        //rotY = Mathf.Clamp(rotY, -360f, 360f);
        rotX = Mathf.Clamp(rotX, minRotX, maxRotX);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);

    }
}
