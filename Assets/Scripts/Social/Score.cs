using System.Threading.Tasks;
using Parse;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Social
{
    public class Score : MonoBehaviour
    {
        [SerializeField]
        private string trainingLeaderboardName = "TrainingScores";

        [SerializeField]
        private string cadetLeaderboardName = "CadetScores";

        [SerializeField]
        private string primeLeaderboardName = "PrimeScores";

        [SerializeField]
        private string commandLeaderboardName = "CommandScores";

        void Start()
        {
            UnityEngine.Social.localUser.Authenticate(ProcessAuthentication);
        }

        public void WriteTrainingRecord(long record)
        {
            UnityEngine.Social.ReportScore(record, trainingLeaderboardName, ProcessReportScore);
        }

        public void WriteCadetRecord(long record)
        {
            UnityEngine.Social.ReportScore(record, cadetLeaderboardName, ProcessReportScore);
        }

        public void WritePrimeRecord(long record)
        {
            UnityEngine.Social.ReportScore(record, primeLeaderboardName, ProcessReportScore);
        }

        public void WriteCommandRecord(long record)
        {
            UnityEngine.Social.ReportScore(record, commandLeaderboardName, ProcessReportScore);
        }


        void ProcessAuthentication(bool success)
        {
            if(success)
            {
                Debug.Log("Authentication success");

                // start read scores
                UnityEngine.Social.LoadScores(trainingLeaderboardName, ProcessTrainingScores);
                UnityEngine.Social.LoadScores(cadetLeaderboardName, ProcessCadetScores);
                UnityEngine.Social.LoadScores(primeLeaderboardName, ProcessPrimeScores);
                UnityEngine.Social.LoadScores(commandLeaderboardName, ProcessCommandScores);
            }
        }

        void ProcessTrainingScores(IScore[] scores)
        {
            throw new System.NotImplementedException();
        }

        void ProcessCadetScores(IScore[] scores)
        {
            throw new System.NotImplementedException();
        }

        void ProcessPrimeScores(IScore[] scores)
        {
            throw new System.NotImplementedException();
        }

        void ProcessCommandScores(IScore[] scores)
        {
            throw new System.NotImplementedException();
        }

        void ProcessReportScore(bool success)
        {
            throw new System.NotImplementedException();
        }
    }
}
