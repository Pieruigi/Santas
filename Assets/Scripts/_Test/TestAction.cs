using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.ar.santas.test
{
    public class TestAction : MonoBehaviour
    {
        public Santa santa;

        public List<Transform> destinations;

        int destCount = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        void Move()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(destinations.Count > destCount)
                {
                    santa.AddDestination(destinations[destCount].position);
                    destCount++;
                }
                
            }
        }
    }

}
