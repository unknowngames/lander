using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class TutorualSpawnZone : SpawnZone
    {
        public override Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}