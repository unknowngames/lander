using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISpaceship
    {
        string Name { get; set; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Velosity { get; }
        Vector3 AngularVelosity { get; }

        float Mass { get; set; }
        float RemainingFuel { get; set; }
        float ThrottleLevel { get; set; }
        float EnginePower { get; }
        bool IsCrashed { get; }

        bool IsPaused { get; set; }

        void Reset ();
    }
}