using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.ar.santas
{
    public class PlayerController : MonoBehaviour
    {
        public UnityAction<GameObject> OnSantaSelected;
        public UnityAction<GameObject> OnSantaUnselected;
        public UnityAction<Gift> OnGiftHighlighted;
        public UnityAction OnLeftClick;

        public static PlayerController Instance { get; private set; }

        HelperPlaneController helperPlaneController;
        HelperRayController helperRayController;

        bool settingHorizontal = false;
        bool settingVertical = false;
        Vector3 targetDestination;
        
        /// <summary>
        /// The object we have clicked on ( santa, gift, building, ecc )
        /// </summary>
        GameObject selectedSanta;

        float rayDistance = 1000;
        int destinationStep = 0;
        

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                helperPlaneController = GetComponent<HelperPlaneController>();
                helperRayController = GetComponent<HelperRayController>();
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            LeftMouseInput();
            RightMouseInput();

            if (settingHorizontal)
            {
                SetHorizontal();
            }
            else
            {
                if (settingVertical)
                {
                    SetVertical();
                }
            }
        }

        void LeftMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClick?.Invoke();

                if(settingHorizontal || settingVertical)
                {
                    settingVertical = false;
                    settingHorizontal = false;
                    helperPlaneController.Show(false);
                    helperRayController.Show(false);
                }


                // Cast a ray from the camera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, rayDistance))
                {
                    Debug.Log("HIt:" + hitInfo.transform.gameObject);
                    if (Tags.Santa.Equals(hitInfo.transform.tag))
                    {

                        GameObject hit = hitInfo.transform.gameObject;
                        GameObject old = null;
                        if(selectedSanta != null)
                        {
                            // We must unselect the current santa
                            old = selectedSanta;
                            selectedSanta = null;
                            OnSantaUnselected?.Invoke(old);
                            Debug.LogFormat("Santa {0} unselected", old);
                        }

                        // We try select the new santa 
                        if(old != hit)
                        {
                            selectedSanta = hit;
                            OnSantaSelected?.Invoke(hit);
                            Debug.LogFormat("Santa {0} selected", selectedSanta);
                        }



                    }
                    else
                    {
                        if (Tags.Gift.Equals(hitInfo.transform.tag))
                        {
                            Gift gift = hitInfo.transform.GetComponentInParent<Gift>();

                            OnGiftHighlighted?.Invoke(gift);
                        }
                    }
                }
                else
                {
                    // Deselect santa

                    if (selectedSanta)
                    {
                        OnSantaUnselected?.Invoke(selectedSanta);
                        selectedSanta = null;
                    }
                }
            }
        }

        void RightMouseInput()
        {
            if (CameraController.Instance.Mode == CameraMode.Free)
                return;

            if (Input.GetMouseButtonDown(1))
            {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                bool found = false;

                // Check for gift or building first
                // If we are manually setting the path we must deactivate click on gifts and buildings
                if (!settingVertical && !settingHorizontal)
                {
                    LayerMask mask = LayerMask.GetMask(new string[] { Layers.TacticsPlane, Layers.IgnoreRaycast });

                    if (Physics.Raycast(ray, out hitInfo, rayDistance, ~mask))
                    {

                        GameObject hit = hitInfo.transform.gameObject;
                        Debug.Log("Hit:" + hit);

                        // Check gift
                        if (Tags.Gift.Equals(hit.tag))
                        {
                            Debug.Log("Gift");
                            found = true;
                            if (selectedSanta)
                            {
                                AddDestinationToSelectedSanta(hit.transform.parent.position);
                            }
                        }
                        else
                        {
                            // Check building
                            if (Tags.Building.Equals(hit.tag))
                            {
                                found = true;
                                // Add destination to the seleceted santa, if any
                                if (selectedSanta)
                                {
                                    AddDestinationToSelectedSanta(hit.transform.parent.GetComponent<Building>().Target.position);
                                    //if (!Input.GetKey(KeyCode.LeftControl))
                                    //{
                                    //    selectedSanta.GetComponent<Santa>().RemoveAllDestinations();
                                    //}
                                    //selectedSanta.GetComponent<Santa>().AddDestination(hit.transform.parent.GetComponent<Building>().Target.position);
                                }
                                
                            }
                        }

                    }
                }


                if (found)
                    return;

                 // Raycast
                 //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, rayDistance))
                {
                    GameObject hit = hitInfo.transform.gameObject;

                    if (Tags.Floor.Equals(hit.tag))
                    {
                        if (selectedSanta)
                        {
                            
                            if(!settingHorizontal && !settingVertical)
                            {
                                // We just started to set direction
                                // Add the helper plane
                                Vector3 pos = selectedSanta.transform.position;
                                helperPlaneController.Show(true);
                                helperPlaneController.SetPosition(pos);
                                settingHorizontal = true;
                            }
                            else
                            {
                                if (settingHorizontal)
                                {
                                    // Set the horizontal position
                                    settingHorizontal = false;
                                    settingVertical = true;
                                }
                                else
                                {
                                    if (settingVertical)
                                    {
                                        settingHorizontal = false;
                                        settingVertical = false;
                                        // Remove the helpers
                                        helperPlaneController.Show(false);
                                        helperRayController.Show(false);
                                        // Add destination
                                        if (selectedSanta) // Should be selected, but... who knows
                                        {
                                            AddDestinationToSelectedSanta(targetDestination);
                                        }
                                        //if (!Input.GetKey(KeyCode.LeftControl))
                                        //{
                                        //    selectedSanta.GetComponent<Santa>().RemoveAllDestinations();
                                        //}
                                        //selectedSanta.GetComponent<Santa>().AddDestination(targetDestination);
                                        

                                    }
                                }
                            }
                           
                            

                        }
                    }

                }

          
            }
        }

        void AddDestinationToSelectedSanta(Vector3 destination)
        {
            if (!selectedSanta)
                return;
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                selectedSanta.GetComponent<Santa>().RemoveAllDestinations();
            }
            selectedSanta.GetComponent<Santa>().AddDestination(destination);
        }

        void SetHorizontal()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, rayDistance))
            {
                GameObject hit = hitInfo.transform.gameObject;
                if (Tags.Floor.Equals(hit.tag))
                {
                    targetDestination = hitInfo.point;
                    Bounds bounds = LevelManager.Instance.GetBounds();
                    targetDestination.x = Mathf.Clamp(targetDestination.x, bounds.min.x, bounds.max.x);
                    targetDestination.z = Mathf.Clamp(targetDestination.z, bounds.min.z, bounds.max.z);

                    // Update helper
                    helperRayController.Show(true);
                    helperRayController.SetPositionCount(2);
                    Vector3 pos = selectedSanta.transform.position;
                    helperRayController.SetPosition(0, pos);
                    helperRayController.SetPosition(1, targetDestination);
                }
            }
        }

        void SetVertical()
        {
            float v = Input.GetAxis("Mouse Y");
            Debug.Log("V:" + v);
            if (v != 0)
            {
                targetDestination += Vector3.up * v;
                Bounds bounds = LevelManager.Instance.GetBounds();
                targetDestination.y = Mathf.Clamp(targetDestination.y, bounds.min.y, bounds.max.y);

                helperRayController.SetPositionCount(3);
                Vector3 pos = selectedSanta.transform.position;
                helperRayController.SetPosition(0, pos);
                helperRayController.SetPosition(2, targetDestination);
            }
        }
    }

}
