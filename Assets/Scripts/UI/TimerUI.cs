using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.ar.santas.UI
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField]
        Text clock;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            clock.text = string.Format("{0}", (int)LevelManager.Instance.TimeLimit);
        }
    }

}
