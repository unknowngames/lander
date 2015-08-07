using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField]
        private Rect spawnZone;

        public Vector3 GetRandomPosition()
        {
            Vector3 self = transform.position;

            float x = spawnZone.x + Random.Range(self.x - spawnZone.width / 2.0f, self.x + spawnZone.width / 2.0f);
            float y = spawnZone.y + Random.Range(self.y - spawnZone.height / 2.0f, self.y + spawnZone.height / 2.0f);
            float z = transform.position.z;
            return new Vector3(x, y, z);
        }

        public void OnDrawGizmosSelected()
        {
            Vector3 center = new Vector3(spawnZone.x, spawnZone.y, 0);
            Vector3 size = new Vector3(spawnZone.width, spawnZone.height, 0);

            Color old = Gizmos.color;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + center, size);
            Gizmos.color = old;
        }
    }
}
