using Assets.Scripts.Interfaces;
using Assets.Scripts.Session;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ResetButtonActions : MonoBehaviour
    {
        [SerializeField]
        private GameSessionStorage gameSessionStorage;

        private IGameSessionStorage GameSessionStorage
        {
            get { return gameSessionStorage; }
        }

        public void OnReset()
        {
            GameSessionStorage.RemoveSavedGame();
        }
    }
}