using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.ar.santas.UI
{
    public class SantaUI : MonoBehaviour
    {
        // The santa that owns this ui
        [SerializeField]
        GameObject owner;

        [SerializeField]
        GameObject panel;

        [SerializeField]
        List<GiftUI> gifts;

        private void Awake()
        {
            owner.GetComponent<Santa>().OnGiftDelivered += HandleOnGiftDelivered;
            owner.GetComponent<Santa>().OnGiftCollected += HandleOnGiftCollected;
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerController.Instance.OnSantaSelected += HandleOnSantaSelected;
            PlayerController.Instance.OnSantaUnselected += HandleOnSantaUnselected;

                       
            
            panel.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerController.Instance.OnSantaSelected -= HandleOnSantaSelected;
            PlayerController.Instance.OnSantaUnselected -= HandleOnSantaUnselected;
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

        void FillGifts(Santa santa)
        {
            // Reset the gift list
            foreach (GiftUI gift in gifts)
            {
                gift.ResetGift();
            }
           
            // Fill gift list
            for(int i=0; i<santa.Gifts.Count; i++)
            {
                gifts[i].SetGift(santa.Gifts[i]);
            }

        }

        void HandleOnSantaSelected(GameObject santa)
        {
            if (owner != santa)
                return;

            // Activate the main panel
            panel.SetActive(true);



            // Fill gift list
            FillGifts(santa.GetComponent<Santa>());
        }

        void HandleOnSantaUnselected(GameObject santa)
        {
            if (santa != owner)
                return;

            panel.SetActive(false);
        }

        void HandleOnGiftDelivered(Santa santa, Gift gift)
        {
            // This should not really happen
            if (santa.gameObject != owner)
                return;

            // Fill ui
            FillGifts(santa);
        }

        void HandleOnGiftCollected(Santa santa, Gift gift)
        {
            // This should not really happen
            if (santa.gameObject != owner)
                return;

            // Fill ui
            FillGifts(santa);
        }
    }

}
