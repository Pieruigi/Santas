using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.ar.santas
{
    public class Building : MonoBehaviour
    {
        public UnityAction<Building, Gift> OnGiftDropped;

        // The available gift colors for this building
        [SerializeField]
        List<Gift> neededGifts;
        public IList<Gift> NeededGifts
        {
            get { return neededGifts.AsReadOnly(); }
        }

        [SerializeField]
        Transform target;
        public Transform Target
        {
            get { return target; }
        }

        float elapsed = 0;
        float dropTime = 0.5f;

        // All the gitfs santas dropped into this building
        List<Gift> droppedGifts = new List<Gift>();
        public IList<Gift> DroppedGifts
        {
            get { return droppedGifts; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool IsFull()
        {
            if (droppedGifts.Count == neededGifts.Count)
                return true;
            else
                return false;
        }

        public bool NeedsGift(Gift gift)
        {
            Debug.LogFormat("building {0} needs {1}", name, gift.name);
            if (neededGifts.Contains(gift))
                return true;
            else
                return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Tags.Santa.Equals(other.tag))
            {
                // Reset elapsed
                elapsed = 0;
                
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Tags.Santa.Equals(other.tag))
            {
                // Drop gifts
                elapsed += Time.fixedDeltaTime;

                // We don't want to drop all the gifts at the same time
                if (elapsed > dropTime)
                {
                    elapsed = 0;
                    Gift dropped = null;
                    if(other.GetComponent<Santa>().TryDropGift(this, out dropped))
                    {
                        droppedGifts.Add(dropped);
                        OnGiftDropped?.Invoke(this, dropped);
                    }
                }
                
            }
        }
    }

}
