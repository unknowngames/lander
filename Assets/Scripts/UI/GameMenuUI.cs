using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameMenuUI : MenuUI
    {
        [SerializeField]
        private SpaceshipControllerUI controllerUIPrefab;

        private SpaceshipControllerUI controllerUIInstance;

        protected override void Awake()
        {
            base.Awake();

            controllerUIInstance = Instantiate(controllerUIPrefab);
            controllerUIInstance.transform.SetParent(transform, false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            controllerUIInstance.Show();
        }

        public void OnPause()
        {
            GameHelper.Pause();
        }
    }
}