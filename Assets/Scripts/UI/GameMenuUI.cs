using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameMenuUI : MenuUI
    {
        [SerializeField]
        private SpaceshipControllerUI controllerUIInstance;

        protected override void Awake()
        {
            base.Awake();
            controllerUIInstance.transform.localPosition = Vector3.zero;
        }

        public override void Show()
        {
            base.Show();

            controllerUIInstance.Show();
        }

        public void OnPause()
        {
            GameHelper.Pause();
        }
    }
}