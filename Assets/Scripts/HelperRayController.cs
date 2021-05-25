using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ar.santas
{
    public class HelperRayController : MonoBehaviour
    {
        [SerializeField]
        GameObject helperRayPrefab;

        LineRenderer lineRend;

        private void Awake()
        {
            lineRend = GameObject.Instantiate(helperRayPrefab).GetComponent<LineRenderer>();            
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
            lineRend.gameObject.SetActive(value);
        }

        public void SetPositionCount(int count)
        {
            lineRend.positionCount = count;
        }

        public void SetPosition(int index, Vector3 position)
        {
            lineRend.SetPosition(index, position);
        }
    }

}
