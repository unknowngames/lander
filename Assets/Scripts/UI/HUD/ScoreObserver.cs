using Assets.Scripts.Session;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class ScoreObserver : UIBehaviour
    {                        
        [SerializeField]
        private Text scoreText;

        public void Update()
        {
            string format = System.String.Format("Score: {0}", ScoreCalculator.Current.CurrentScore.ScorePoints);
            if (scoreText != null)
            {
                scoreText.text = format;
            }
        }
    }
}