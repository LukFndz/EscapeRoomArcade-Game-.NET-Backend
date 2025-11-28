using Assets.Scripts.Player;
using Assets.Scripts.Api;
using UnityEngine;
using Assets.Scripts.Score;
using Assets.Scripts.UI;

namespace Assets.Scripts.Game
{
    public sealed class GameEndController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private EndGameUI _endUI;
        [SerializeField] private GameTimer _timer;


        private void OnEnable()
        {
            _timer.OnTimerEnd += HandleEnd;
            _timer.OnTimerStop += HandleWin;
        }

        private void OnDisable()
        {
            _timer.OnTimerEnd -= HandleEnd;
            _timer.OnTimerStop -= HandleWin;
        }

        private void HandleEnd()
        {
            _player.DisableInput();
            Time.timeScale = 0f;
            _endUI.ShowLose();
        }

        private void HandleWin()
        {
            _player.DisableInput();
            Time.timeScale = 0f;
            _endUI.ShowWin();
        }
    }

    [System.Serializable]
    public struct EndRunPayload
    {
        public string playerName;
        public int objectsPushed;
        public int coinsEarned;
        public bool win;
    }
}
