using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.ar.santas
{
    public enum CameraMode { Free, Tactics }

    public class CameraController : MonoBehaviour
    {

        public UnityAction<CameraMode> OnModeSwitched;

        public static CameraController Instance { get; private set; }

        [SerializeField]
        BoxCollider borders;
        
        //[SerializeField]
        //float xMinLimit;
        //[SerializeField]
        //float xMaxLimit;

        //[SerializeField]
        //float yMinLimit;
        //[SerializeField]
        //float yMaxLimit;

        //[SerializeField]
        //float zMinLimit;
        //[SerializeField]
        //float zMaxLimit;

        CameraMode mode = CameraMode.Free;
        public CameraMode Mode
        {
            get { return mode; }
        }

        //bool tacticsMode = false;
        //public bool TacticsMode
        //{
        //    get { return tacticsMode; }
        //}

        float moveSpeed = 5;

        float rotX;
        float rotY;
        float minRotX = -90;
        float maxRotX = 90;
        float mouseSens = 50f;

        bool switchingMode = false;
        float switchTime = 0.2f;
        Vector3 targetPosition;

        Vector3 switchVelocity;
        Vector3 switchAnglesVelocity;
        Vector3 targetAngles;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                borders.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            rotX = transform.eulerAngles.x;
            rotY = transform.eulerAngles.y;
        }

        // Update is called once per frame
        void Update()
        {


            if (switchingMode)
            {

      
                    
                // Camrea transition
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref switchVelocity, switchTime);
                transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, targetAngles, ref switchAnglesVelocity, switchTime);
             

                // Check has completed
                if ((transform.position - targetPosition).sqrMagnitude < 0.001f )
                {
                    switchingMode = false;
                    OnModeSwitched?.Invoke(mode);
                }

                return;
            }


            // Switch from free to tactics camera           
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                switchingMode = true;
                switchVelocity = Vector3.zero;
                switchAnglesVelocity = Vector3.zero;
                //targetPosition = transform.position;
                targetAngles = transform.eulerAngles;
                
               
                if (mode == CameraMode.Free)
                {
                    mode = CameraMode.Tactics;
                    targetPosition.x = (borders.bounds.max.x + borders.bounds.min.x) / 2f;
                    targetPosition.z = (borders.bounds.max.z + borders.bounds.min.z) / 2f;
                    targetPosition.y = borders.bounds.max.y;
                    rotX = 70;
                    targetAngles.x = rotX;
                }
                else
                {
                    mode = CameraMode.Free;
                    targetPosition = transform.position;
                    targetPosition.y -= (borders.bounds.max.y - borders.bounds.min.y) * 0.3f;
                    rotX = 50;
                    targetAngles.x = rotX;
                }
                return;   
            }

       
            targetPosition = ComputeTargetPosition();

            // Check if the camera is in switching mode
           
            // Set new position
           
            transform.position = targetPosition;


            FreeLook();



        }





        Vector3 ComputeTargetPosition()
        {
            Vector3 moveDisp = Vector3.zero;

            // Move forward and back
            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            
            if(Input.GetAxis("Vertical") != 0)// && mode == CameraMode.Free)
            {
                // We don't want the cam to horizontally ( not really along its fwd axis )
                Vector3 dir = transform.forward;
                dir.y= 0;
                dir.Normalize();

                Debug.Log("Move dir:" + dir);

                // Move
                //transform.position += dir * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
                moveDisp += dir * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
                Debug.Log("Move disp:" + moveDisp);
            }

            // Move left and right
            if (Input.GetAxis("Horizontal") != 0)
            {
                // Get the direction
                Vector3 dir = transform.right;
                
                // Move
                //transform.position += dir * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
                moveDisp += dir * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; 
            }

            // Move up and down
            if (Input.GetAxis("Height") != 0)// && mode == CameraMode.Free)
            {
                Vector3 dir = Vector3.up;
        
                // Move
                //transform.position += dir * Input.GetAxis("Height") * moveSpeed * Time.deltaTime;
                moveDisp += dir * Input.GetAxis("Height") * -moveSpeed * Time.deltaTime;
            }

            // Compute target position
            Vector3 targetPosition = transform.position + moveDisp;

            // Clamp target position
            Vector3 min = borders.bounds.min;
            Vector3 max = borders.bounds.max;
            targetPosition.x = Mathf.Clamp(targetPosition.x, min.x, max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, min.y, max.y);
            targetPosition.z = Mathf.Clamp(targetPosition.z, min.z, max.z);

            return targetPosition;
            
        }

        void FreeLook()
        {
            // Check input
            rotX -= Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
            rotY += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
            
            // Clamp angles
            //rotY = Mathf.Clamp(rotY, -360f, 360f);
            rotX = Mathf.Clamp(rotX, minRotX, maxRotX);

            transform.rotation = Quaternion.Euler(rotX, rotY, 0);
           
        }

        

        

    }

}
