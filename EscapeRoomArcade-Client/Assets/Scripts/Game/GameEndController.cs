using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public sealed class GameEndController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private GameObject _endgamePanel;
        [SerializeField] private PlayerController _player;
        #endregion

        #region Monobehaviour Functions
        private void OnEnable()
        {
            GameTimer.OnTimerEnd += HandleEnd;
        }

        private void OnDisable()
        {
            GameTimer.OnTimerEnd -= HandleEnd;
        }
        #endregion

        #region Private Functions
        private void HandleEnd()
        {
            _player.DisableInput();
            _endgamePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        #endregion
    }
}