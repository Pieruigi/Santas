using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.ar.santas
{


    public class Santa : MonoBehaviour
    {

        public UnityAction<Santa, Gift> OnGiftDelivered;
        public UnityAction<Santa, Gift> OnGiftCollected;
        public UnityAction<Santa> OnDestroying;
        
        float speed = 2;
        float giftPenalty = 0.2f;

        List<Vector3> destinations = new List<Vector3>();
        Vector3 currentDestination;
        bool hasDestination = false;

        int giftCountMax = 5;
        List<Gift> gifts = new List<Gift>();
        public IList<Gift> Gifts
        {
            get { return gifts; }
        }

        float rotSpeed = 10f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Santa is not doing anything
            if (!hasDestination)
            {
                // Look for a new action to perform
                if(TryPopDestination(out currentDestination))
                {
                    // Found, let's do it
                    hasDestination = true;
                    Debug.Log("Desination set: " + currentDestination);
                }
            }
            
            if(hasDestination)
            {
                // Santa is doing something
                Move();

                // Adjust fwd
                Vector3 targetFwd = (currentDestination - transform.position).normalized;
                transform.forward = Vector3.MoveTowards(transform.forward, targetFwd, rotSpeed * Time.deltaTime);
            }

            
        }

        private void OnDestroy()
        {
            OnDestroying?.Invoke(this);
        }

        public bool IsFull()
        {
            return !(gifts.Count < giftCountMax);
        }

        public bool TryCollect(Gift gift)
        {
            if (gifts.Count >= giftCountMax)
                return false;

            gifts.Add(gift);
            OnGiftCollected?.Invoke(this, gift);
            return true;

        }

        public void AddDestination(Vector3 destination)
        {
            destinations.Add(destination);
            
        }

        public void RemoveAllDestinations()
        {
            destinations.Clear();
            hasDestination = false;
        }

        public bool TryPopDestination(out Vector3 destination)
        {
            destination = Vector3.zero;
            if (destinations.Count == 0)
                return false;

            destination = destinations[0];
            // Remove from the list
            destinations.RemoveAt(0);
            return true;
        }

        public bool TryDropGift(Building building, out Gift gift)
        {
            gift = null;
            List<Gift> neededGifts = new List<Gift>(building.NeededGifts);
            foreach(Gift needed in neededGifts)
            {
                gift = gifts.Find(g => g == needed);
                if (gift)
                {
                    gifts.Remove(gift);
                    OnGiftDelivered?.Invoke(this, gift);
                    return true;
                }
            }

            return false;
        }

        

        void Move()
        {
            float actualSpeed = speed - gifts.Count * giftPenalty;
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, actualSpeed * Time.deltaTime);

            if ((transform.position - currentDestination).sqrMagnitude < Mathf.Epsilon)
            {
                Debug.Log("Distination reached:" + transform.position);
                    
                hasDestination = false;
            }
                
        }
    }

}
