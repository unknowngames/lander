using System;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class UINavigator : UIBehaviour
    {                                                            
        [SerializeField]
        private SpaceshipControllerUI controllerUIPrefab;

        private SpaceshipControllerUI controllerUIInstance;

        [SerializeField]
        private MainMenuUI mainMenuUIPrefab;

        private MainMenuUI mainMenuUIInstance;

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

            controllerUIInstance = Instantiate (controllerUIPrefab);
            controllerUIInstance.transform.SetParent (transform, false);

            pauseMenuUIInstance = Instantiate (pauseMenuUIPrefab);
            pauseMenuUIInstance.transform.SetParent (transform, false);

            resultMenuUIInstance = Instantiate(resultMenuUIPrefab);
            resultMenuUIInstance.transform.SetParent(transform, false);

            mainMenuUIInstance = Instantiate(mainMenuUIPrefab);
            mainMenuUIInstance.transform.SetParent(transform, false);

            gameMenuUIInstance = Instantiate(gameMenuUIPrefab);
            gameMenuUIInstance.transform.SetParent(transform, false);

            Show (mainMenuUIInstance);
        }

        private void Show (params MenuUI[] menus)
        {   
            HideAll ();

            foreach (MenuUI menuUI in menus)
            {
                menuUI.Show ();
            }
        }

        private void HideAll()
        {
            controllerUIInstance.Hide();
            pauseMenuUIInstance.Hide();
            resultMenuUIInstance.Hide();
            mainMenuUIInstance.Hide();
            gameMenuUIInstance.Hide ();
        }

        protected override void OnEnable ()
        {
            base.OnEnable ();
            Subscribe ();
        }

        protected override void OnDisable ()
        {
            base.OnDisable ();
            Unsubscribe ();
        }

        private void Subscribe()
        {
            Game.OnBegin += OnBeginGame;
            Game.OnAbort += OnAbortGame;
            Game.OnFinish += OnFinishGame;
            Game.OnPause += OnPauseGame;
            Game.OnUnpause += OnUnpauseGame;
        }

        private void Unsubscribe()
        {
            Game.OnBegin -= OnBeginGame;
            Game.OnAbort -= OnAbortGame;
            Game.OnFinish -= OnFinishGame;
            Game.OnPause -= OnPauseGame;
            Game.OnUnpause -= OnUnpauseGame;  
        }

        private void OnBeginGame(object sender, EventArgs eventArgs)
        {          
            Show(gameMenuUIInstance, controllerUIInstance);
        }

        private void OnAbortGame(object sender, EventArgs e)
        {
            Show (mainMenuUIInstance);
        }

        private void OnFinishGame(object sender, EventArgs e)
        {
            Show (resultMenuUIInstance);
        }

        private void OnPauseGame (object sender, EventArgs e)
        {
            Show(pauseMenuUIInstance);
        }

        private void OnUnpauseGame(object sender, EventArgs e)
        {
            Show(gameMenuUIInstance, controllerUIInstance);
        }
    }
}