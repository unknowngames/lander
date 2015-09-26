using UnityEngine;

namespace Assets.Scripts.Environment
{
    public abstract class SpawnZone : MonoBehaviour
    {                             
        public abstract Vector3 GetPosition();
    }
}