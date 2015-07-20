using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Spaceship;

public class RocketEngineAnimator : MonoBehaviour 
{
    [Serializable]
    private class ParticleSystemParameters
    {
        [SerializeField]
        private float minStartSize;
        [SerializeField]
        private float maxStartSize;

        [SerializeField]
        private float minStartSpeed;
        [SerializeField]
        private float maxStartSpeed;

        [SerializeField]
        private float initialYPozition;
        [SerializeField]
        private float deltaYPozition;


        public float MinStartSize
        {
            get { return minStartSize; }
        }

        public float MaxStartSize
        {
            get { return maxStartSize; }
        }

        public float MinStartSpeed
        {
            get { return minStartSpeed; }
        }

        public float MaxStartSpeed
        {
            get { return maxStartSpeed; }
        }

        public float InitialYPozition
        {
            get { return initialYPozition; }
        }

        public float DeltaYPozition
        {
            get { return deltaYPozition; }
        }
    }

    [SerializeField]
    private SpaceshipBehaviour spaceship;

    [SerializeField]
    private ParticleSystem main;
    [SerializeField]
    private ParticleSystem glow;

    [SerializeField]
    private ParticleSystemParameters mainParameters;
    [SerializeField]
    private ParticleSystemParameters glowParameters;

    [SerializeField]
    private bool debugMode;

    [SerializeField]
    [Range(0, 1)]   
    private float throttleLevel;

    public void Update ()
    {
        if (!debugMode)
        {
            throttleLevel = spaceship.EnginePower;
        }

        main.startSpeed = (mainParameters.MaxStartSpeed - mainParameters.MinStartSpeed) * throttleLevel;
        main.startSize = (mainParameters.MaxStartSize - mainParameters.MinStartSize) * throttleLevel;

        Vector3 mainLocalPosition = main.transform.localPosition;
        mainLocalPosition.y = mainParameters.InitialYPozition + mainParameters.DeltaYPozition * throttleLevel;
        main.transform.localPosition = mainLocalPosition;

        
        glow.startSpeed = (glowParameters.MaxStartSpeed - glowParameters.MinStartSpeed) * throttleLevel;
        glow.startSize = (glowParameters.MaxStartSize - glowParameters.MinStartSize) * throttleLevel;

        Vector3 glowLocalPosition = glow.transform.localPosition;
        glowLocalPosition.y = glowParameters.InitialYPozition + glowParameters.DeltaYPozition * throttleLevel;
        glow.transform.localPosition = glowLocalPosition;
    }   
}
