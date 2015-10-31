using System.Threading.Tasks;
using Parse;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Social
{
    public class Score : MonoBehaviour
    {
		[SerializeField]
		Scripts.Settings.GameDifficultyStorage difficultyStorage;

        void Start()
        {
            UnityEngine.Social.localUser.Authenticate(ProcessAuthentication);
        }

        public void Write(string leaderboardID, long record)
        {
			UnityEngine.Social.ReportScore(record, leaderboardID, processReportScore);
        }

		void processReportScore(bool success)
		{
			Debug.Log ("Top score report status: " + success);
		}

        void ProcessAuthentication(bool success)
        {
            if(success)
            {
                Debug.Log("Social platform authentication success");

				var diffs = difficultyStorage.Difficulties;

				foreach(var diff in diffs)
				{
					UnityEngine.Social.LoadScores(diff.LeaderboardID, ProcessScores);
				}
            }
        }

        void ProcessScores(IScore[] scores)
        {
			Debug.Log (scores.Length);
        }
    }
}
