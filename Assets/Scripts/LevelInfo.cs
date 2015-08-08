using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelInfo : MonoBehaviour, ILevelInfo
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private float gravitation;
        [SerializeField]
        private float atmosphericDrag;


        public string Name
        {
            get { return name; }
        }
        public float Gravitation
        {
            get { return gravitation; }
        }
        public float AtmosphericDrag
        {
            get { return atmosphericDrag; }
        }
    }
}