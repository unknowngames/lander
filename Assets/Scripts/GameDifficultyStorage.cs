using System.Linq;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameDifficultyStorage : ScriptableObject, IGameDifficultyStorage
    {
        [SerializeField]
        private ClassicGameDifficulty[] difficulties;

        public IGameDifficulty[] Difficulties
        {
            get { return difficulties.Cast<IGameDifficulty>().ToArray(); }
        }

        public IGameDifficulty this[string name]
        {
            get
            {
                foreach (ClassicGameDifficulty difficulty in difficulties)
                {
                    if (difficulty.Name.Equals(name))
                    {
                        return difficulty;
                    }
                }
                return null;
            }
        }

        public bool IsExist(string name)
        {
            foreach (ClassicGameDifficulty difficulty in difficulties)
            {
                if (difficulty.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}