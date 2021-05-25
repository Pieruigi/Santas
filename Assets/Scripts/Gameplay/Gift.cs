using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ar.santas
{
    

    public class Gift : MonoBehaviour
    {

    
        private void Awake()
        {
         
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (Tags.Santa.Equals(other.tag))
            {
                // Get santa component
                Santa santa = other.GetComponent<Santa>();

                // If santa is carrying to many gifts then returns...
                if (santa.IsFull())
                    return;

                // ... otherwise add the new gift
                santa.TryCollect(this);

                // Move gift over santa
                transform.parent = santa.gameObject.transform;
                transform.localPosition = Vector3.zero;

                // Hide gift
                gameObject.SetActive(false);
            }
        }


    }

}
