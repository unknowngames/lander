
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.UI
{
    public class ResultMenuUI : MenuUI
    {
        public UnityEngine.UI.Text SuccessLandingScoreLabel;
        public UnityEngine.UI.Text SuccessLandingScore;

        public UnityEngine.UI.Text SoftLandingScoreLabel;
        public UnityEngine.UI.Text SoftLandingScore;

        public UnityEngine.UI.Text PreciseLandingScoreLabel;
        public UnityEngine.UI.Text PreciseLandingScore;

        protected override void OnEnable()
        {
            base.OnEnable();

            SuccessLandingScore.text = Game.Instance.CurrentScore.ScorePoints.ToString();
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