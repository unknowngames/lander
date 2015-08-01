using System;
using System.Collections;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class GameUINavigator : UIBehaviour
    {                                                         
        [SerializeField]
        private PauseMenuUI pauseMenuUIPrefab;

        private PauseMenuUI pauseMenuUIInstance;

        [SerializeField]
        private GameMenuUI gameMenuUIPrefab;

        private GameMenuUI gameMenuUIInstance;

        [SerializeField]
        private ResultMenuUI resultMenuUIPrefab;

        private ResultMenuUI resultMenuUIInstance;


        protected override void Awake ()
        {
            base.Awake ();

            pauseMenuUIInstance = Instantiate (pauseMenuUIPrefab);
            pauseMenuUIInstance.transform.SetParent (transform, false);

            resultMenuUIInstance = Instantiate(resultMenuUIPrefab);
            resultMenuUIInstance.transform.SetParent(transform, false);
                                                                       
            gameMenuUIInstance = Instantiate(gameMenuUIPrefab);
            gameMenuUIInstance.transform.SetParent(transform, false);

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