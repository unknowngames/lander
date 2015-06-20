using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISpaceship
    {
        string Name { get; set; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 Velosity { get; set; }
        Vector3 AngularVelosity { get; set; }

        float Mass { get; set; }
        float RemainingFuel { get; set; }
    }
}