using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.ar.santas.UI
{
    public class BuildingUI : MonoBehaviour
    {
        // The santa that owns this ui
        [SerializeField]
        GameObject owner;

        [SerializeField]
        GameObject panel;

        [SerializeField]
        List<Image> gifts;

        int next = 0;

        private void Awake()
        {
            owner.GetComponent<Building>().OnGiftDropped += HandleOnGiftDropped;          
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerController.Instance.OnGiftHighlighted += HandleOnGiftHighlighted;
            PlayerController.Instance.OnLeftClick += HandleOnLeftClick;

            panel.SetActive(false);
        }

        private void OnDestroy()
        {
 
        }

        // Update is called once per frame
        void Update()
        {
            
            if (panel.activeSelf)
            {
                // Billboard effect
                panel.transform.LookAt(Camera.main.transform.position);
            }
        }

        public void GiftHighlighted(Gift gift)
        {
            if (owner.GetComponent<Building>().NeedsGift(gift))
                panel.SetActive(true);
            else
                panel.SetActive(false);
        }

       

        
        void HandleOnGiftHighlighted(Gift gift)
        {
            GiftHighlighted(gift);
        }
       

        void HandleOnGiftDropped(Building building, Gift gift)
        {
            gifts[next].color = Color.green;
            next++;
        }

        void HandleOnLeftClick()
        {
            panel.SetActive(false);
        }
       
    }

}
