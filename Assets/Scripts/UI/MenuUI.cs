using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
	public class MenuUI : UIBehaviour
    {
        [SerializeField]
        private SpaceshipControllerUI controllerUIPrefab;

        private SpaceshipControllerUI controllerUIInstance;

	    protected SpaceshipControllerUI ControllerUIInstance
	    {
	        get
            {
                if (controllerUIInstance == null)
                {
                    controllerUIInstance = Instantiate(controllerUIPrefab);
                    controllerUIInstance.transform.parent = transform;
                    controllerUIInstance.transform.position = Vector3.zero;

                    controllerUIInstance.gameObject.SetActive(false);
                }

                return controllerUIInstance;
            }
	    }

        public void ShowUIControllers(ISpaceshipMoveable moveable)
	    {
            ControllerUIInstance.SpaceshipMoveable = moveable;
            ControllerUIInstance.gameObject.SetActive(true);
        }

        public void HideUIControllers()
	    {
            ControllerUIInstance.gameObject.SetActive(false);
	    }
	}
}
