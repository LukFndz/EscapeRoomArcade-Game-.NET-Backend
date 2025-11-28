using UnityEngine;
using System;

namespace Assets.Scripts.Score
{
    public sealed class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public static event Action<int> OnScoreChanged;

        public int TotalObjectsPushed { get; private set; }
        public int TotalCoinsEarnedThisRun { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ResetRun()
        {
            TotalObjectsPushed = 0;
            TotalCoinsEarnedThisRun = 0;

            OnScoreChanged?.Invoke(TotalCoinsEarnedThisRun);
        }

        public void AddScore(int amount)
        {
            TotalObjectsPushed++;
            TotalCoinsEarnedThisRun += amount;
            OnScoreChanged?.Invoke(TotalCoinsEarnedThisRun);
        }

        public void AddCoins(int amount)
        {
            TotalCoinsEarnedThisRun += amount;
        }

        public void RegisterObjectPushed(int coinValue)
        {
            TotalObjectsPushed++;
            TotalCoinsEarnedThisRun += coinValue;

            TotalCoinsEarnedThisRun += coinValue;
            OnScoreChanged?.Invoke(TotalCoinsEarnedThisRun);
        }
    }
}
