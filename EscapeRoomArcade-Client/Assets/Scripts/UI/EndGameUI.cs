using UnityEngine;
using TMPro;
using Assets.Scripts.Api;
using Assets.Scripts.Score;
using Assets.Scripts.Player;
using System.Collections;
using Assets.Scripts.Game;

namespace Assets.Scripts.UI
{
    public class EndGameUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text leaderboardText;
        [SerializeField] private TMP_Text yourRankText;
        [SerializeField] private PlayerController player;

        [Header("Settings")]
        [SerializeField] private bool showDebugLogs = false;

        private bool _win;

        private void Start()
        {
            panel.SetActive(false);
        }

        public void ShowLose()
        {
            _win = false;
            ShowPanel();
            statusText.text = "You get trapped!";
            StartCoroutine(LoadLeaderboard());
        }

        public void ShowWin()
        {
            _win = true;
            ShowPanel();
            statusText.text = "You saved the coins!";
            SendRunToServer();
        }

        private void ShowPanel()
        {
            Time.timeScale = 0f;
            player.DisableInput();
            panel.SetActive(true);
        }

        private void SendRunToServer()
        {
            var payload = new EndRunPayload
            {
                playerName = LoginManager.Instance.CurrentPlayerName,
                objectsPushed = ScoreManager.Instance.TotalObjectsPushed,
                coinsEarned = ScoreManager.Instance.TotalCoinsEarnedThisRun,
                win = _win
            };

            string json = JsonUtility.ToJson(payload);

            StartCoroutine(ApiClient.Instance.EndRun(json, (ok, msg) =>
            {
                if (showDebugLogs)
                    Debug.Log("END SUBMIT RESULT = " + msg);

                StartCoroutine(LoadLeaderboard());
            }));
        }

        private IEnumerator LoadLeaderboard()
        {
            leaderboardText.text = "Loading leaderboard...";
            yourRankText.text = "Loading rank...";

            // LEADERBOARD
            yield return ApiClient.Instance.GetLeaderboard((ok, json) =>
            {
                if (!ok)
                {
                    leaderboardText.text = "Failed to load leaderboard.";
                    return;
                }

                string wrapped = JsonHelper.WrapArray(json);
                var entries = JsonHelper.FromJson<LeaderboardEntryDto>(wrapped);

                leaderboardText.text = "";

                int count = Mathf.Min(entries.Length, 10);
                for (int i = 0; i < count; i++)
                {
                    leaderboardText.text += $"{i + 1}. {entries[i].playerName} - {entries[i].totalCoins}\n";
                }
            });

            // PLAYER RANK
            string name = LoginManager.Instance.CurrentPlayerName;
            yield return ApiClient.Instance.GetRank(name, (ok, json) =>
            {
                if (!ok)
                {
                    yourRankText.text = "Rank: N/A";
                    return;
                }

                var rankDto = JsonUtility.FromJson<PlayerRankDto>(json);
                yourRankText.text = $"Your Rank: #{rankDto.rank}";
            });
        }

        public void Retry()
        {
            Time.timeScale = 1f;
            StopAllCoroutines();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }

        public void GoToMenu()
        {
            Time.timeScale = 1f;
            StopAllCoroutines();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }


    [System.Serializable]
    public struct LeaderboardEntryDto
    {
        public string playerName;
        public int totalCoins;
    }

    [System.Serializable]
    public struct PlayerRankDto
    {
        public string playerName;
        public int rank;
    }
}
