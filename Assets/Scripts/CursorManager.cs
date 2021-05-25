using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ar.santas
{
    public class CursorManager : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {
            // We start in free mode, so we hide the cursor
            Cursor.lockState = CursorLockMode.Locked;

            // Set handle to the camera
            CameraController.Instance.OnModeSwitched += delegate (CameraMode cameraMode) 
            {
                switch (cameraMode)
                {
                    case CameraMode.Free:
                        Cursor.lockState = CursorLockMode.Locked;
                        break;
                    case CameraMode.Tactics:
                        Cursor.lockState = CursorLockMode.None;
                        break;
                }
                    
            };
        }

        // Update is called once per frame
        void Update()
        {

        }



    }

}
