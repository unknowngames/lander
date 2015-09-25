using Assets.Scripts.Spaceship;
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

        float LeftStabilizerThrottleLevel { get; set; }
        float RightStabilizerThrottleLevel { get; set; }

        float EnginePower { get; }

        float LeftStabilizerEnginePower { get; }
        float RightStabilizerEnginePower { get; }

        bool IsCrashed { get; }
        bool IsLanded { get; }

        TouchdownTrigger TouchdownTrigger { get; }

        void Reset ();

        ISpaceshipState Save();
        void Restore(ISpaceshipState state);
    }
}