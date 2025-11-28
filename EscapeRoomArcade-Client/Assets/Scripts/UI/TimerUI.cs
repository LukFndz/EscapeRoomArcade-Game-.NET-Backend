using Assets.Scripts.Game;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public sealed class TimerUI : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private GameTimer _timer;
        [SerializeField] private TMP_Text _text;
        #endregion

        #region Monobehaviour Functions
        private void Update()
        {
            _text.text = Mathf.Ceil(_timer.Remaining).ToString();
        }
        #endregion
    }
}