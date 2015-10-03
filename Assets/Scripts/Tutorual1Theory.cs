using System;
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
            public HighlightedEntity HighlightTransform;

            [SerializeField]
            [Multiline]
            public string Text;

            [SerializeField]
            public float Delay;
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
            if (steps[currentStep].HighlightTransform != null)
            {
                steps[currentStep].HighlightTransform.Stop();
            }

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
            helpMenuUI.ShowStaticHelpDuringTime(tutorialStep.Text, tutorialStep.Delay);
            if (tutorialStep.HighlightTransform != null)
            {
                tutorialStep.HighlightTransform.Do();
            }
        }
    }
}