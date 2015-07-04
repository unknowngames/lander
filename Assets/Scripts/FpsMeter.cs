using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FpsMeter : MonoBehaviour
    {

        // Attach this to a GUIText to make a frames/second indicator.
        //
        // It calculates frames/second over each updateInterval,
        // so the display does not keep changing wildly.
        //
        // It is also fairly accurate at very low FPS counts (<10).
        // We do this not by simply counting frames per interval, but
        // by accumulating FPS for each frame. This way we end up with
        // correct overall FPS even if the interval renders something like
        // 5.5 frames.

        public float updateInterval = 0.5F;

        private float accum = 0; // FPS accumulated over the interval
        private int frames = 0; // Frames drawn over the interval
        private float timeleft; // Left time for current interval
        private Text text;

        void Start()
        {
            text = GetComponent<Text> ();
            if (!GetComponent<Text>())
            {
                Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
                enabled = false;
                return;
            }
            timeleft = updateInterval;
        }

        void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            if (timeleft <= 0.0)
            {
                float fps = accum / frames;
                string format = System.String.Format("{0:F2} FPS", fps);
                text.text = format;

                if (fps < 30)
                    text.material.color = Color.yellow;
                else
                    if (fps < 10)
                        text.material.color = Color.red;
                    else
                        text.material.color = Color.green;
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }
}