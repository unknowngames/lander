using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

        private Coroutine tickerCoroutine;
        private bool isShown = false;

        protected override void Start()
        {         
            base.Start();
            HideHelp();
        }

        public void ShowHelp(string text, int count)
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

        public void ShowHelpDuringTime(string text, float time)
        {
            HideHelp();

            ticker.Text = text;
            ticker.Show();

            if (time > 0.0f)
            {
                tickerCoroutine = StartCoroutine(TickerTimerCoroutine(time));
            }

            isShown = true;
        }

        public void ShowHelp(string text)
        {
            ShowHelpDuringTime(text, 0.0f);
        }

        public void HideHelp()
        {
            if (!isShown)
            {
                return;
            }

            ticker.Hide();
            ticker.RewindEvent.RemoveAllListeners();

            if (tickerCoroutine != null)
            {
                StopCoroutine(tickerCoroutine);
                tickerCoroutine = null;
            }
            isShown = false;

            HelpHideEvent.Invoke();
        }

        private IEnumerator TickerTimerCoroutine(float time)
        {                                                  
            yield return new WaitForSeconds(time);
            HideHelp();
        }

        [ContextMenu("Show test 5 sec")]
        public void ShowTest5Sec()
        {
            ShowHelp("1123123", 5);
        }

        [ContextMenu("Show test")]
        public void ShowTest()
        {
            ShowHelp("1123123");
        }

        [ContextMenu("Hide test")]
        public void HideTest()
        {
            HideHelp();
        }
    }
}
