using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.ar.santas.UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        Transform buttonContent;


        // Start is called before the first frame update
        void Start()
        {
            // Init buttons
            Button[] buttons = buttonContent.GetComponentsInChildren<Button>();
            for(int i=0; i<buttons.Length; i++)
            {
                if (ProgressManager.Instance.NextLevel >= i)
                    buttons[i].interactable = true;
                else
                    buttons[i].interactable = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                ProgressManager.Instance.TryUpdateProgress(0);
        }

        public void LoadLevel(int levelId)
        {
            // The first scene in the build list is the main scene
            SceneManager.LoadScene(levelId + Constants.LevelIdStartBuildIndex);
        }

        public void ApplicationQuit()
        {
            Application.Quit();
        }
    }

}
