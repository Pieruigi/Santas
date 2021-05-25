using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ar.santas
{
    public class HelperPlaneController : MonoBehaviour
    {
        [SerializeField]
        GameObject helperPlanePrefab;

        GameObject helperPlane;

        private void Awake()
        {
            helperPlane = GameObject.Instantiate(helperPlanePrefab);
            helperPlane.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show(bool value)
        {
            helperPlane.SetActive(value);
        }

        public void SetPosition(Vector3 position)
        {
            helperPlane.transform.position = position;
        }

        public Vector3 GetPosition()
        {
            return helperPlane.transform.position;
        }
    }

}
