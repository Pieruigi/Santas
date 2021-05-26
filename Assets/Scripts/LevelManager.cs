using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace com.ar.santas
{

    public class LevelManager : MonoBehaviour
    {
        public UnityAction OnPlayerWins;
        public UnityAction OnPlayerLoses;

        public static LevelManager Instance { get; private set; }

        [SerializeField]
        GameObject boundHelper;

        [SerializeField]
        float timeLimit = 150f;
        public float TimeLimit
        {
            get { return timeLimit; }
        }

        [SerializeField]
        int minimumGitfs = 10;

        Vector3 minBounds;
        Vector3 maxBounds;

        BoxCollider boundsColl;
        List<Santa> santas;
        List<Gift> gifts;

        int deliveredCount = 0;

        int levelId;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                boundsColl = boundHelper.GetComponent<BoxCollider>();
                boundHelper.GetComponent<MeshRenderer>().enabled = false;
                levelId = SceneManager.GetActiveScene().buildIndex - Constants.LevelIdStartBuildIndex;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get all santas to check if player loses
            santas = new List<Santa>(GameObject.FindObjectsOfType<Santa>());
            foreach(Santa santa in santas)
            {
                santa.OnDestroying += HandleOnStantaKidnapped;
                santa.OnGiftDelivered += HandleOnGiftDelivered;
            }

            // Get all gift to check if level is completed
            gifts = new List<Gift>(GameObject.FindObjectsOfType<Gift>());
        }

        // Update is called once per frame
        void Update()
        {
            timeLimit -= Time.deltaTime;

            if(timeLimit < 0)
            {
                if(deliveredCount >= minimumGitfs)
                {
                    Win();
                }
                else
                {
                    OnPlayerLoses?.Invoke();
                }
            }
        }

        public Bounds GetBounds()
        {
            return boundsColl.bounds;
        }

        void HandleOnStantaKidnapped(Santa santa)
        {
            santas.Remove(santa);
            if(santas.Count == 0)
            {
                // You lose
                Debug.Log("No santa left, you lose");
                
                OnPlayerLoses?.Invoke();
            }
        }

        void HandleOnGiftDelivered(Santa santa, Gift gift)
        {
            // Remove gift from list
            gifts.Remove(gift);

            // Update counter
            deliveredCount++;

            // If no gifts are left then you win
            if(gifts.Count == 0)
            {
                Debug.Log("You win");
                Win();
            }
        }

        void Win()
        {
            ProgressManager.Instance.TryUpdateProgress(levelId);
            OnPlayerWins?.Invoke();
        }
    }
}
