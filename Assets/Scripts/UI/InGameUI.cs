using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.ar.santas.UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField]
        GameObject youWinText;

        [SerializeField]
        GameObject youLoseText;

        [SerializeField]
        GameObject panel;

        private void Awake()
        {
            youLoseText.SetActive(false);
            youWinText.SetActive(false);
            panel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            LevelManager.Instance.OnPlayerWins += HandleOnPlayerWins;
            LevelManager.Instance.OnPlayerLoses += HandleOnPlayerLoses;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadMainScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(Constants.MainSceneBuildIndex);
        }

        void HandleOnPlayerWins()
        {
            Time.timeScale = 0;
            panel.SetActive(true);
            youWinText.SetActive(true);
        }

        void HandleOnPlayerLoses()
        {
            Time.timeScale = 0;
            panel.SetActive(true);
            youLoseText.SetActive(true);
            
        }

        
    }

}
