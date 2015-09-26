using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class GameUINavigator : UIBehaviour
    {                                                         
        [SerializeField]                           
        private PauseMenuUI pauseMenuUIInstance;

        [SerializeField]
        private GameMenuUI gameMenuUIInstance;

        [SerializeField]
        private ResultMenuUI resultMenuUIInstance;


        protected override void Start ()
        {
            base.Start();

            pauseMenuUIInstance.transform.localPosition = Vector3.zero;
            gameMenuUIInstance.transform.localPosition = Vector3.zero;
            resultMenuUIInstance.transform.localPosition = Vector3.zero;

            Show(gameMenuUIInstance);
        }


        private void Show(params MenuUI[] menus)
        {
            Show(0.0f, menus);
        }

        private void Show(float timeout, params MenuUI[] menus)
        {
            StartCoroutine(ShowIE(timeout, menus));
        }
        
        private IEnumerator ShowIE (float timeout, params MenuUI[] menus)
        {   
            HideAll ();

            yield return new WaitForSeconds(timeout);
            
            foreach (MenuUI menuUI in menus)
            {
                menuUI.Show ();
            }
        }

        private void HideAll()
        {
            pauseMenuUIInstance.Hide();
            resultMenuUIInstance.Hide();
            gameMenuUIInstance.Hide ();
        }

        protected override void OnEnable ()
        {
            base.OnEnable ();
            Subscribe ();
        }
             
        private void Subscribe()
        {
            GameHelper.OnBegin.AddListener (OnBeginGame);
            GameHelper.OnFinish.AddListener (OnFinishGame);
            GameHelper.OnPause.AddListener (OnPauseGame);
            GameHelper.OnUnpause.AddListener(OnUnpauseGame);
            GameHelper.OnMissionCompleted.AddListener(OnMissionCompleted);
            GameHelper.OnAbort.AddListener(OnAbortGame);
        }

        private void OnBeginGame ()
        {          
            Show(gameMenuUIInstance);
        }

        private void OnFinishGame()
        {
            Show(1.5f, resultMenuUIInstance);
        }

        private void OnMissionCompleted()
        {
            Show(1.5f, resultMenuUIInstance);
        }

        private void OnPauseGame ()
        {
            Show(pauseMenuUIInstance);
        }

        private void OnUnpauseGame()
        {
            Show(gameMenuUIInstance);
        }

        private void OnAbortGame()
        {
            HideAll();
        }
    }
}