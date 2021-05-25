using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ar.santas
{
    public class Befana : MonoBehaviour
    {
        
        Vector3 destination;
        //bool chasingSanta = false;

       
        float moveSpeed = 0.6f;
        float detectionRange = 6;
        float attackRange = 2;
        float sqrDetectionRange;
        float sqrAttackRange;

        List<GameObject> santas;
        GameObject engagedSanta;

        private void Awake()
        {
            // Set destination as reached
            destination = transform.position;

            sqrDetectionRange = detectionRange * detectionRange;
            sqrAttackRange = attackRange * attackRange;
        }

        // Start is called before the first frame update
        void Start()
        {
            santas = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Santa));
            // Add handles
            foreach(GameObject santa in santas)
            {
                santa.GetComponent<Santa>().OnDestroying += HandleOnSantaDestroying;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Try to engage one of the santas 
            if (!engagedSanta)
            {
                TryEngageSanta(out engagedSanta);
            }
            else
            {
                // Check for disengagement
                float sqrDist = (engagedSanta.transform.position - transform.position).sqrMagnitude;
                if (sqrDist > sqrDetectionRange)
                    engagedSanta = null; // Disengaged
            }
                
    
            if (!engagedSanta)
            {
                // Not chasing any santa, just patrol randomly
                // Check if last destination has been reached
                if (DestinationReached())
                {
                    // Befana needs a new destination
                    SetDestination(GetRandomDestination());
                }
            }
            else
            {
                // Check the attack range
                float sqrDist = (transform.position - engagedSanta.transform.position).sqrMagnitude;
                if(sqrDist < sqrAttackRange)
                {
                    // Kidnap santa
                    // Remove from the list
                    santas.Remove(engagedSanta);
                    Destroy(engagedSanta);
                    engagedSanta = null;
                }
                else
                {
                    // Follow santa
                    SetDestination(engagedSanta.transform.position);
                }
                
            }

            // Move to destination
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        }

        void SetDestination(Vector3 destination)
        {
            this.destination = destination;
        }

        bool DestinationReached()
        {
            return Vector3.Distance(transform.position, destination) < Mathf.Epsilon;
        }

        Vector3 GetRandomDestination()
        {
            Bounds bounds = LevelManager.Instance.GetBounds();
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            float z = Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(x, y, z);
        }

        bool TryEngageSanta(out GameObject engaged)
        {
            engaged = null;
            if (engagedSanta)
            {
                engaged = engagedSanta;
                return true;
            }
                
            foreach(GameObject santa in santas)
            {
                if((santa.transform.position - transform.position).sqrMagnitude < sqrDetectionRange)
                {
                    engaged = santa;
                    return true;
                }
            }

            return false;
        }
        
        void HandleOnSantaDestroying(Santa santa)
        {
            santas.Remove(santa.gameObject);
            
        }
        

    }

}
