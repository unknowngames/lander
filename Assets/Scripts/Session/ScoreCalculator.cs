using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Session
{
    public class ScoreCalculator : MonoBehaviour, IScoreCalculator
    {
        public static ScoreCalculator Current { get; private set; }

        private IGameSession lastSession;
        private GameScore currentScore = GameScore.Create(0,0);
		private float initialFuel;
		private float landingStartTime;
        private float currentLandingTime = 0;

        [SerializeField]
        private float fuelBonusFactor = 0.0066f;

        public float FuelBonusFactor
        {
            get
            {
                return fuelBonusFactor;
            }
        }

        public IGameScore CurrentScore
        {
            get { return currentScore; }
        }

        public void SetInitialScore(IGameSession gameSession)
        {
            lastSession = gameSession;
            currentScore = GameScore.Create(lastSession.Score);
        }

        protected void OnEnable()
        {
            if (Current == null)
            {
                Current = this;
            }
            else
            {
                Debug.LogWarning("Multiple ScoreCalculator in scene... this is not supported");
            }
        }

        protected void OnDisable()
        {
            if (Current == this)
            {
                Current = null;
            }
        }

        public void Update()
        {
            if (Flight.Current.IsPaused == false) 
			{
				currentLandingTime += Time.deltaTime;
			}
        }

		public void Begin()
		{
            initialFuel = PlayerSpawner.PlayerSpaceship.RemainingFuel;
			currentLandingTime = 0;
		}

        public IGameSession Calculate()
        {
            ISpaceshipState lastState = PlayerSpawner.PlayerSpaceship.Save();
            SpaceshipState newState = SpaceshipState.Create(lastState);

            if (PlayerSpawner.PlayerSpaceship.IsLanded)
            {
                int totalScore = 0;

                // расчет бонуса за время посадки

                // расчет бонуса за мягкую посадку
                float softLandingScore = 0;

                var collisions = PlayerSpawner.PlayerSpaceship.Collisions;
                bool isLandingSoft = true;
                const float softCollisionVelocity = 0.5f;
                float maxSoftCollision = 0.0f;
                

                foreach(var c in collisions)
                {
                    var magnitude = c.relativeVelocity.magnitude;
                    if (magnitude >= softCollisionVelocity)
                    {
                        isLandingSoft = false;
                        break;
                    }
                    else if(magnitude > maxSoftCollision)
                    {
                        maxSoftCollision = magnitude;
                    }
                }

                if (isLandingSoft)
                {
                    if(maxSoftCollision <= softCollisionVelocity / 2.0f)
                        softLandingScore += 2000.0f + 2000.0f * (1.0f - maxSoftCollision / (softCollisionVelocity / 2.0f));
                    else if(maxSoftCollision <= softCollisionVelocity)
                        softLandingScore += 1000.0f + 1000.0f * (1.0f - maxSoftCollision / softCollisionVelocity);
                }

                UnityEngine.Debug.Log("Soft landed : " + isLandingSoft + " collisions: " + collisions.Count + " velocity magn: " + maxSoftCollision);

                if (collisions.Count <= 1)
                    softLandingScore += 1000;

                currentScore.SoftLandingScore = (int)softLandingScore;
                totalScore += (int)softLandingScore;

                // расчет очков за точность посадки
                float preciseLandingScore = 0;

                float distanceFromCenter = Mathf.Abs(PlayerSpawner.PlayerSpaceship.transform.position.x - PlayerSpawner.PlayerSpaceship.TouchdownTrigger.Zone.transform.position.x);

                if (distanceFromCenter <= 0.5f)
                    preciseLandingScore += 2000.0f + 2000.0f * (1.0f - distanceFromCenter / 0.5f);
                else if (distanceFromCenter <= 2.0f)
                    preciseLandingScore += 1000.0f + 1000.0f * (1.0f - distanceFromCenter / 2.0f);

                UnityEngine.Debug.Log("Precise landing score: " + preciseLandingScore + " Distance: " + distanceFromCenter);
                currentScore.PreciseLandingScore = (int)preciseLandingScore;
                totalScore += (int)preciseLandingScore;

				// расчет очков за время посадки
				int landingTimeScore = (int)(120000.0f / currentLandingTime);
				currentScore.LandingTimeScore = landingTimeScore;
				totalScore += landingTimeScore;
				UnityEngine.Debug.Log("Landing time: " + currentLandingTime + " Score " + landingTimeScore);


				// fuel score
                float consumedFuel = initialFuel - PlayerSpawner.PlayerSpaceship.RemainingFuel;
				Debug.Log("Consumed fuel " + consumedFuel);
				float fuelConsumptionScore = (200000.0f / consumedFuel);
				totalScore += (int)fuelConsumptionScore;
				currentScore.FuelConsumptionScorePoints = fuelConsumptionScore;
                
                // расчет очков за успешную посадку
                int successLandingScore = 5000 * PlayerSpawner.PlayerSpaceship.TouchdownTrigger.Zone.ScoreMultiplier;
                totalScore += successLandingScore;

                currentScore.SuccessLandingScore = successLandingScore;
                currentScore.ScorePoints += totalScore;
				currentScore.LandingTime = currentLandingTime;
                currentScore.LandingsCount++;

                Debug.Log("Total score " + totalScore);

                currentScore.FuelBonus = totalScore * FuelBonusFactor;
                newState.RemainingFuel += currentScore.FuelBonus;
            }

            if (PlayerSpawner.PlayerSpaceship.IsCrashed)
            {
                currentScore.ScorePoints += 5;
            }

            return GameSession.Create(newState, currentScore);
        }
    }
}