using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public class GameDifficultyStorage : ScriptableObject, IGameDifficultyStorage
    {
        private const string DifficultyKey = "DifficultyKey";
        
        [SerializeField]
        private ClassicGameDifficulty[] difficulties;

        public IGameDifficulty[] Difficulties
        {
            get { return difficulties.Cast<IGameDifficulty>().ToArray(); }
        }

        public IGameDifficulty this[string difficultyName]
        {
            get
            {
                foreach (IGameDifficulty difficulty in difficulties)
                {
                    if (difficulty.Name.Equals(difficultyName))
                    {
                        return difficulty;
                    }
                }
                throw new KeyNotFoundException();
            }
        }

        IGameDifficulty IGameDifficultyStorage.this[int index]
        {
            get { return difficulties[index]; }
        }

        public bool IsExist(string name)
        {
            foreach (IGameDifficulty difficulty in difficulties)
            {
                if (difficulty.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetIndex(string difficultyName)
        {
            for (int index = 0; index < difficulties.Length; index++)
            {
                IGameDifficulty difficulty = difficulties[index];
                if (difficulty.Name.Equals(difficultyName))
                {
                    return index;
                }
            }
            throw new KeyNotFoundException();
        }

        public int GetIndex(IGameDifficulty difficulty)
        {
            return GetIndex(difficulty.Name);
        }

        public int DifficultiesCount
        {
            get { return difficulties.Length; }
        }

        public void ApplyDifficulty(IFlight flight)
        {
            IGameDifficulty savedDifficulty = GetSavedDifficulty();
            savedDifficulty.Apply(flight);
        }

        public IGameDifficulty GetSavedDifficulty()
        {
            if (PlayerPrefsX.HasKey(DifficultyKey))
            {
                string key = PlayerPrefsX.GetString(DifficultyKey);
                if (IsExist(key))
                {
                    return this[key];
                }
            }
            return Difficulties[0];
        }

        public void SetDifficulty(IGameDifficulty difficulty)
        {
            if (IsExist(difficulty.Name))
            {
                PlayerPrefsX.SetString(DifficultyKey, difficulty.Name);
            }
        }
    }
}