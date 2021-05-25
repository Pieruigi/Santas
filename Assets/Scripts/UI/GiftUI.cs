using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ar.santas.UI
{
    public class GiftUI : MonoBehaviour, IPointerDownHandler
    {
        Gift gift;

        Image image;

        BuildingUI[] buildingsUIs;

        bool highlightBuilding = false;

        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();

            buildingsUIs = GameObject.FindObjectsOfType<BuildingUI>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            if (highlightBuilding)
            {
                highlightBuilding = false;

                foreach (BuildingUI ui in buildingsUIs)
                    ui.GiftHighlighted(gift);
            }
        }

        public void SetGift(Gift gift)
        {
            this.gift = gift;
            image.color = Color.green;
        }

        public void ResetGift()
        {
            gift = null;
            image.color = Color.white;
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (gift == null)
                return;

            // We call actual highlight in the late update to avoid the playercontroller to reset 
            // building ui OnLeftClick()
            highlightBuilding = true;

           
        }
    }

}
