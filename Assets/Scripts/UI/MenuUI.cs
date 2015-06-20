using Assets.Scripts.Controllers;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class MenuUI : UIBehaviour
    {
        private SpaceshipControllerUI controllerUIInstance;

        [SerializeField]
        private SpaceshipControllerUI controllerUIPrefab;

        protected SpaceshipControllerUI ControllerUIInstance
        {
            get
            {
                if (controllerUIInstance == null)
                {
                    controllerUIInstance = Instantiate (controllerUIPrefab);
                    controllerUIInstance.transform.SetParent (transform, false);

                    controllerUIInstance.gameObject.SetActive (false);
                }

                return controllerUIInstance;
            }
        }

        public void ShowUIControllers (ISpaceshipMoveable moveable)
        {
            ControllerUIInstance.SpaceshipMoveable = moveable;
            ControllerUIInstance.gameObject.SetActive (true);
        }

        public void HideUIControllers ()
        {
            ControllerUIInstance.gameObject.SetActive (false);
        }
    }
}