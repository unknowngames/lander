
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.UI
{
    public class ResultMenuUI : MenuUI
    {
        /// <summary>
        /// Скорость роста очков
        /// </summary>
        public float ScoreGrowSpeed = 30;

        public GameObject SuccessPanel;

        public GameObject FailPanel;
        public UnityEngine.UI.Text FailText;

		public UnityEngine.UI.Text ElapsedTimeLabel;
		public UnityEngine.UI.Text ElapsedTime;

        public UnityEngine.UI.Text SuccessLandingScoreLabel;
        public UnityEngine.UI.Text SuccessLandingScore;

        public UnityEngine.UI.Text SoftLandingScoreLabel;
        public UnityEngine.UI.Text SoftLandingScore;

        public UnityEngine.UI.Text PreciseLandingScoreLabel;
        public UnityEngine.UI.Text PreciseLandingScore;

		public UnityEngine.UI.Text LandingTimeScoreLabel;
		public UnityEngine.UI.Text LandingTimeScore;

		public UnityEngine.UI.Text FuelScoreLabel;
		public UnityEngine.UI.Text FuelScore;

		public UnityEngine.UI.Text TotalScoreLabel;
		public UnityEngine.UI.Text TotalScore;

        protected override void OnEnable()
        {
            base.OnEnable();

            if(GameHelper.CurrentScore.SuccessLandingScore > 0)
            {
                SuccessPanel.SetActive(true);
                FailPanel.SetActive(false);
                StartCoroutine(scoreGrow());
            }
            else
            {
                FailPanel.SetActive(true);
                SuccessPanel.SetActive(false);
            }
        }

        System.Collections.IEnumerator scoreGrow()
        {
            SuccessLandingScore.text = "0";
            SoftLandingScore.text = "0";
            PreciseLandingScore.text = "0";
			LandingTimeScore.text = "0";
			FuelScore.text = "0";

			ElapsedTime.text = string.Format("{0:F2}", GameHelper.CurrentScore.LandingTime) + " сек.";

            float growSpeed = 1.0f / ScoreGrowSpeed;

            int success = GameHelper.CurrentScore.SuccessLandingScore;
            int currentSuccess = 0;

            while(currentSuccess < success)
            {
                currentSuccess++;
                SuccessLandingScore.text = currentSuccess.ToString();
                yield return new WaitForSeconds(growSpeed);
            }

            int soft = GameHelper.CurrentScore.SoftLandingScore;
            int currrentSoft = 0;

            while (currrentSoft < soft)
            {
                currrentSoft++;
                SoftLandingScore.text = currrentSoft.ToString();
                yield return new WaitForSeconds(growSpeed);
            }

            int precise = GameHelper.CurrentScore.PreciseLandingScore;
            int currentPrecise = 0;

            while (currentPrecise < precise)
            {
                currentPrecise++;
                PreciseLandingScore.text = currentPrecise.ToString();
                yield return new WaitForSeconds(growSpeed);
            }

			int landingTimeScore = GameHelper.CurrentScore.LandingTimeScore;
			int currentLandingTime = 0;

			while (currentLandingTime < landingTimeScore) 
			{
				currentLandingTime++;
				LandingTimeScore.text = currentLandingTime.ToString();
				yield return new WaitForSeconds(growSpeed);
			}

			int cachedFuelScore = (int)GameHelper.CurrentScore.FuelConsumptionScore;
			int currentFuelScore = 0;
			
			while (currentFuelScore < cachedFuelScore) 
			{
				currentFuelScore++;
				FuelScore.text = currentFuelScore.ToString();
				yield return new WaitForSeconds(growSpeed);
			}

			int totalScore = landingTimeScore + success + precise + soft;
			int currentTotalScore = 0;
			
			while (currentTotalScore < totalScore) 
			{
				currentTotalScore++;
				TotalScore.text = currentTotalScore.ToString();
				yield return new WaitForSeconds(growSpeed);
			}
        }

        public void OnStart()
        {      
            GameHelper.Begin ();
        }

        public void OnAbort()
        {
            GameHelper.Abort();
        }
    }
}