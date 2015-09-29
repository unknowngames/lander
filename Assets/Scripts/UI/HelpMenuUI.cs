using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HelpMenuUI : MenuUI
    {
        [SerializeField]
        private Ticker ticker;

        private Coroutine tickerCoroutine;

        protected override void Start()
        {         
            base.Start();
            HideHelp();
        }

        public void ShowHelp(string text, float time)
        {
            HideHelp();

            ticker.Text = text;
            ticker.Show();

            if (time > 0.0f)
            {
                tickerCoroutine = StartCoroutine(TickerTimerCoroutine(time));
            }
        }

        public void ShowHelp(string text)
        {
            ShowHelp(text, 0.0f);
        }

        public void HideHelp()
        {
            ticker.Hide();

            if (tickerCoroutine != null)
            {
                StopCoroutine(tickerCoroutine);
                tickerCoroutine = null;
            }
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
