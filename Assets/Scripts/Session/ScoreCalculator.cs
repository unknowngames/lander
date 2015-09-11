using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly IGame game;

        private IGameSession lastSession;
        private GameScore current = GameScore.Create(0,0,0,0,0);


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
            //current.ScorePoints++;
        }

        public IGameSession Calculate()
        {
            ISpaceshipState lastState = game.PlayerSpaceship.Save();
            SpaceshipState newState = SpaceshipState.Create(lastState);

            if (game.PlayerSpaceship.IsLanded)
            {
                int totalScore = 0;

                // ������ ������ �� ����� �������

                // ������ ������ �� ������ �������
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

                // ������ ����� �� �������� �������
                float preciseLandingScore = 0;

                float distanceFromCenter = UnityEngine.Mathf.Abs(game.PlayerSpaceship.transform.position.x - game.PlayerSpaceship.TouchdownTrigger.Zone.transform.position.x);

                if (distanceFromCenter <= 0.5f)
                    preciseLandingScore += 20.0f + 20.0f * (1.0f - distanceFromCenter / 0.5f);
                else if (distanceFromCenter <= 2.0f)
                    preciseLandingScore += 10.0f + 10.0f * (1.0f - distanceFromCenter / 2.0f);

                UnityEngine.Debug.Log("Precise landing score: " + preciseLandingScore + " Distance: " + distanceFromCenter);
                current.PreciseLandingScore = (int)preciseLandingScore;
                totalScore += (int)preciseLandingScore;


                // ������ ����� �� �������� �������
                int successLandingScore = 50 * game.PlayerSpaceship.TouchdownTrigger.Zone.ScoreMultiplier;
                totalScore += successLandingScore;

                current.SuccessLandingScore = successLandingScore;
                current.ScorePoints += totalScore;
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