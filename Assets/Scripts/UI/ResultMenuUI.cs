 using Assets.Scripts.Session;
 using UnityEngine;
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

        public UnityEngine.UI.Text FuelBonusLabel;
        public UnityEngine.UI.Text FuelBonus;

		public UnityEngine.UI.Text NewRecordLabel;
		public UnityEngine.UI.Text NewRecord;

        public override void Show()
        {
            base.Show();

            if (ScoreCalculator.Current.CurrentScore.SuccessLandingScore > 0)
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

		public override void Hide ()
		{
			base.Hide ();

			NewRecordLabel.gameObject.SetActive (false);
		}

        System.Collections.IEnumerator scoreGrow()
        {
            SuccessLandingScore.text = "0";
            SoftLandingScore.text = "0";
            PreciseLandingScore.text = "0";
			LandingTimeScore.text = "0";
            FuelScore.text = "0";
            TotalScore.text = "0";
            FuelBonus.text = "0";

            ElapsedTime.text = string.Format("{0:F2}", ScoreCalculator.Current.CurrentScore.LandingTime) + " сек.";

            float growSpeed = ScoreGrowSpeed;

            int success = ScoreCalculator.Current.CurrentScore.SuccessLandingScore;
            int currentSuccess = 0;

            while(currentSuccess < success)
            {
                currentSuccess += (int)(growSpeed * Time.deltaTime);
                SuccessLandingScore.text = currentSuccess.ToString();
                yield return null;
            }
            currentSuccess = success;
            SuccessLandingScore.text = currentSuccess.ToString();

            int soft = ScoreCalculator.Current.CurrentScore.SoftLandingScore;
            int currrentSoft = 0;

            while (currrentSoft < soft)
            {
                currrentSoft += (int)(growSpeed * Time.deltaTime);
                SoftLandingScore.text = currrentSoft.ToString();
                yield return null;
            }
            currrentSoft = soft;
            SoftLandingScore.text = currrentSoft.ToString();

            int precise = ScoreCalculator.Current.CurrentScore.PreciseLandingScore;
            int currentPrecise = 0;

            while (currentPrecise < precise)
            {
                currentPrecise += (int)(growSpeed * Time.deltaTime);
                PreciseLandingScore.text = currentPrecise.ToString();
                yield return null;
            }
            currentPrecise = precise;
            PreciseLandingScore.text = currentPrecise.ToString();

            int landingTimeScore = ScoreCalculator.Current.CurrentScore.LandingTimeScore;
			int currentLandingTime = 0;

			while (currentLandingTime < landingTimeScore) 
			{
                currentLandingTime += (int)(growSpeed * Time.deltaTime);
				LandingTimeScore.text = currentLandingTime.ToString();
				yield return null;
			}
            currentLandingTime = landingTimeScore;
            LandingTimeScore.text = currentLandingTime.ToString();

            int cachedFuelScore = (int)ScoreCalculator.Current.CurrentScore.FuelConsumptionScorePoints;
			int currentFuelScore = 0;
			
			while (currentFuelScore < cachedFuelScore) 
			{
                currentFuelScore += (int)(growSpeed * Time.deltaTime);
				FuelScore.text = currentFuelScore.ToString();
				yield return null;
			}
            currentFuelScore = cachedFuelScore;
            FuelScore.text = currentFuelScore.ToString();

			int totalScore = ScoreCalculator.Current.CurrentScore.LastFlightScorePoints;
			int currentTotalScore = 0;
			
			while (currentTotalScore < totalScore) 
			{
                currentTotalScore += (int)(growSpeed * Time.deltaTime);
				TotalScore.text = currentTotalScore.ToString();
				yield return null;
			}
            currentTotalScore = totalScore;
            TotalScore.text = currentTotalScore.ToString();

            FuelBonus.text = string.Format("{0:F1}", ScoreCalculator.Current.CurrentScore.FuelBonus);

			NewRecordLabel.gameObject.SetActive (ScoreCalculator.Current.CurrentScore.IsTopScoreBeaten);
			NewRecord.text = ScoreCalculator.Current.CurrentScore.ScorePoints.ToString();
        }
    }
}