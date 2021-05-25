using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ar.santas
{
    public class ProgressManager : MonoBehaviour
    {
        public static ProgressManager Instance { get; private set; }

        int nextLevel = 0;
        public int NextLevel
        {
            get { return nextLevel; }
        }
         
        string saveKey = "save";
        int maxLevel = 9;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                if (PlayerPrefs.HasKey(saveKey))
                    nextLevel = PlayerPrefs.GetInt(saveKey);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {

        }

       

        public bool TryUpdateProgress(int levelId)
        {
            if (levelId == nextLevel && levelId < maxLevel)
            {
                nextLevel++;
                // Save
                PlayerPrefs.SetInt(saveKey, nextLevel);
                PlayerPrefs.Save();
                return true;
            }

            return false;
        }
    }

}
