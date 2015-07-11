using System;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipPart
    {
        public Rigidbody Rigidbody { get; private set; }
        public Transform Transform { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public SpaceshipPart(Rigidbody part)
        {
            Rigidbody = part;
            Transform = Rigidbody.transform;
            Position = Transform.localPosition;
            Rotation = Transform.localRotation;
        }

        public void Reset()
        {
            Transform.localPosition = Position;
            Transform.localRotation = Rotation;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}