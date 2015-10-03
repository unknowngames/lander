using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HelpMenuUI : MenuUI
    {
        [Serializable]
        public class OnHelpHideEvent : UnityEvent{}

        [SerializeField]
        public OnHelpHideEvent HelpHideEvent = new OnHelpHideEvent();

        [SerializeField]
        private Ticker ticker;

        [SerializeField]
        private Text staticText;

        private Coroutine delayCoroutine;
        private bool isShown = false;

        protected override void Awake()
        {
            base.Awake();
            HideHelp(true);
        }

        public void ShowTickerHelp(string text, int count)
        {
            HideHelp();

            ticker.Text = text;
            ticker.Show();

            int shownTime = 0;
            ticker.RewindEvent.AddListener(() =>
            {
                shownTime++;
                if (shownTime >= count)
                {
                    HideHelp();
                }
            });

            isShown = true;
        }

        public void ShowStaticHelpDuringTime(string text, float time)
        {
            ShowHelpDuringTime(text, time, false);
        }

        public void ShowTickerHelpDuringTime(string text, float time)
        {
            ShowHelpDuringTime(text, time, true);
        }

        private void ShowHelpDuringTime(string text, float time, bool useTicker)
        {
            HideHelp();

            if (useTicker)
            {
                ticker.Text = text;
                ticker.Show();
            }
            else
            {
                staticText.text = text;
                staticText.enabled = true;
            }

            if (time > 0.0f)
            {
                delayCoroutine = StartCoroutine(DelayTimerCoroutine(time));
            }

            isShown = true;
        }

        public void ShowStaticHelp(string text)
        {
            ShowHelpDuringTime(text, 0.0f, false);
        }

        public void ShowTickerHelp(string text)
        {
            ShowHelpDuringTime(text, 0.0f, true);
        }

        public void HideHelp(bool force=false)
        {
            if (!(force || isShown))
            {
                return;
            }

            ticker.Hide();
            ticker.RewindEvent.RemoveAllListeners();

            staticText.text = "";
            staticText.enabled = false;

            if (delayCoroutine != null)
            {
                StopCoroutine(delayCoroutine);
                delayCoroutine = null;
            }
            isShown = false;

            HelpHideEvent.Invoke();
        }

        private IEnumerator DelayTimerCoroutine(float time)
        {                                                  
            yield return new WaitForSeconds(time);
            HideHelp();
        }
    }
}
