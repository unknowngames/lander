using UnityEngine;
using System.Collections;
using Assets.Scripts.Spaceship;

public class EngineAnimator : MonoBehaviour 
{
    [SerializeField]
    private SpaceshipBehaviour spaceship;

    [SerializeField]
    private ParticleEmitter emitter;

    public void Update ()
    {
        emitter.maxEmission = spaceship.EnginePower * 10000.0f;
    }   
}
