using System;
using System.Collections;
using Assets.Scripts.Common;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Tutorual1Theory : MonoBehaviour
    {
        [Serializable]
        public class OnTutorialTheoryEndEvent : UnityEvent { }

        [SerializeField]
        public OnTutorialTheoryEndEvent TutorialTheoryEndEvent = new OnTutorialTheoryEndEvent();

        [Serializable]
        private class TutorialStep
        {
            [SerializeField]
            public string Text;
            [SerializeField] 
            public Vector2 HighlightPosition;
            [SerializeField] 
            public float HighlightRadius;
        }

        [SerializeField] 
        private TutorialStep[] steps;

        [SerializeField] 
        private HelpMenuUI helpMenuUI;

        private int currentStep = 0;

        public void Begin()
        {
            helpMenuUI.HelpHideEvent.AddListener(OnHelpHideEvent);
            ShowStep(currentStep);
        }

        private void OnHelpHideEvent()
        {
            currentStep++;
            ShowStep(currentStep);
        }

        private void ShowStep(int i)
        {
            if (i >= steps.Length)
            {
                TutorialTheoryEndEvent.Invoke();
                return;
            }

            ShowStep(steps[i]);
        }

        private void ShowStep(TutorialStep tutorialStep)
        {
            helpMenuUI.ShowHelp(tutorialStep.Text, 1);
        }
    }
}