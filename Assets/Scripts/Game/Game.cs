using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Spaceship;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Game : Singleton<Game>
    {
        [SerializeField]
        private SpaceshipBehaviour spaceshipPrefab;

        [SerializeField]
        private MenuUI menuUI;

        public MenuUI MenuUI
        {
            get
            {
                return menuUI ?? (menuUI = FindObjectOfType<MenuUI> ());
            }
        }

        private SpaceshipBehaviour spaceshipInstance;

        public void Start ()
        {
            spaceshipInstance = Instantiate (spaceshipPrefab);

            MenuUI.ShowUIControllers (spaceshipInstance.GetComponent<ISpaceshipMoveable> ());
        }
    }
}