using Assets.Scripts.Score;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void OnEnable()
        {
            ScoreManager.OnScoreChanged += UpdateUI;
        }

        private void OnDisable()
        {
            ScoreManager.OnScoreChanged -= UpdateUI;
        }

        private void Start()
        {
            UpdateUI(ScoreManager.Instance.TotalCoinsEarnedThisRun);
        }

        private void UpdateUI(int newScore)
        {
            _scoreText.text = "Saved Coins: " + newScore.ToString();
        }
    }
}