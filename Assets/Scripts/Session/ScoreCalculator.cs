using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Session
{
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly IGame game;

        private IGameSession lastSession;
        private GameScore current = GameScore.Create(0,0);
		private float initialFuel;
		private float landingStartTime;
		private float currentLandingTime = 0;

        public ScoreCalculator(IGame game)
        {
            this.game = game;
        }

        public IGameScore Current
        {
            get { return current; }
        }

        public void SetInitialScore(IGameSession gameSession)
        {
            lastSession = gameSession;
            current = GameScore.Create(lastSession.Score);
        }

        public void Update()
        {
            if (game.IsPaused == false) 
			{
				currentLandingTime += Time.deltaTime;
			}
        }

		public void Begin()
		{
			initialFuel = game.PlayerSpaceship.RemainingFuel;
			currentLandingTime = 0;
		}

        public IGameSession Calculate()
        {
            ISpaceshipState lastState = game.PlayerSpaceship.Save();
            SpaceshipState newState = SpaceshipState.Create(lastState);

            if (game.PlayerSpaceship.IsLanded)
            {
                int totalScore = 0;

                // расчет бонуса за время посадки

                // расчет бонуса за мягкую посадку
                float softLandingScore = 0;

                var collisions = game.PlayerSpaceship.Collisions;
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
                        softLandingScore += 20.0f + 20.0f * (1.0f - maxSoftCollision / (softCollisionVelocity / 2.0f));
                    else if(maxSoftCollision <= softCollisionVelocity)
                        softLandingScore += 10.0f + 10.0f * (1.0f - maxSoftCollision / softCollisionVelocity);
                }

                UnityEngine.Debug.Log("Soft landed : " + isLandingSoft + " collisions: " + collisions.Count + " velocity magn: " + maxSoftCollision);

                if (collisions.Count <= 1)
                    softLandingScore += 10;

                current.SoftLandingScore = (int)softLandingScore;
                totalScore += (int)softLandingScore;

                // расчет очков за точность посадки
                float preciseLandingScore = 0;

                float distanceFromCenter = UnityEngine.Mathf.Abs(game.PlayerSpaceship.transform.position.x - game.PlayerSpaceship.TouchdownTrigger.Zone.transform.position.x);

                if (distanceFromCenter <= 0.5f)
                    preciseLandingScore += 20.0f + 20.0f * (1.0f - distanceFromCenter / 0.5f);
                else if (distanceFromCenter <= 2.0f)
                    preciseLandingScore += 10.0f + 10.0f * (1.0f - distanceFromCenter / 2.0f);

                UnityEngine.Debug.Log("Precise landing score: " + preciseLandingScore + " Distance: " + distanceFromCenter);
                current.PreciseLandingScore = (int)preciseLandingScore;
                totalScore += (int)preciseLandingScore;

				// ?????? ????? ?? ????? ???????
				int landingTimeScore = (int)(1200.0f / currentLandingTime);
				current.LandingTimeScore = landingTimeScore;
				totalScore += landingTimeScore;
				UnityEngine.Debug.Log("Landing time: " + currentLandingTime + " Score " + landingTimeScore);


				// fuel score
				float consumedFuel = initialFuel - game.PlayerSpaceship.RemainingFuel;
				Debug.Log("Consumed fuel " + consumedFuel);
				float fuelConsumptionScore = (2000.0f / consumedFuel);
				totalScore += (int)fuelConsumptionScore;
				current.FuelConsumptionScorePoints = fuelConsumptionScore;

                // расчет очков за успешную посадку
                int successLandingScore = 50 * game.PlayerSpaceship.TouchdownTrigger.Zone.ScoreMultiplier;
                totalScore += successLandingScore;

                current.SuccessLandingScore = successLandingScore;
                current.ScorePoints += totalScore;
				current.LandingTime = currentLandingTime;
                current.LandingsCount++;

                newState.RemainingFuel += 50;
            }

            if (game.PlayerSpaceship.IsCrashed)
            {
                current.ScorePoints += 5;
            }

            return GameSession.Create(newState, current);
        }
    }
}