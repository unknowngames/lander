using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly IGame game;

        private IGameSession lastSession;
        private GameScore current;


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

                // расчет бонуса за мягкую посадку
                int softLandingScore = 0;

                var collisions = game.PlayerSpaceship.Collisions;
                bool isLandingSoft = true;
                
                foreach(var c in collisions)
                {
                    if(c.relativeVelocity.magnitude >= 0.25f)
                    {
                        isLandingSoft = false;
                        break;
                    }
                }

                if (isLandingSoft)
                {
                    softLandingScore += 20;
                }

                //UnityEngine.Debug.Log("Soft landed : " + isLandingSoft + " collisions: " + collisions.Count);

                if (collisions.Count <= 1)
                    softLandingScore += 20;

                totalScore += softLandingScore;

                // расчет очков за точность посадки
                int preciseLandingScore = 0;

                float distanceFromCenter = UnityEngine.Mathf.Abs(game.PlayerSpaceship.transform.position.x - game.PlayerSpaceship.TouchdownTrigger.Zone.transform.position.x);

                if (distanceFromCenter <= 0.5f)
                    preciseLandingScore += 40;
                else if (distanceFromCenter <= 1.0f)
                    preciseLandingScore += 10;

                UnityEngine.Debug.Log("Precise landing score: " + preciseLandingScore + " Distance: " + distanceFromCenter);
                totalScore += preciseLandingScore;


                // расчет очков за успешную посадку
                totalScore += 50 * game.PlayerSpaceship.TouchdownTrigger.Zone.ScoreMultiplier;
                
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