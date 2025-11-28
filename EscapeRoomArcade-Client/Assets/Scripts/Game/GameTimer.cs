using System;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public sealed class GameTimer : MonoBehaviour
    {
        #region Events
        public static event Action OnTimerEnd;
        #endregion

        #region Private Variables
        [SerializeField] private float _duration = 30f;

        private float _remaining;
        private bool _running;
        #endregion

        #region Properties
        public float Remaining => _remaining;
        public float Progress => Mathf.Clamp01(1f - (_remaining / _duration));
        #endregion

        #region Monobehaviour Functions
        private void Awake()
        {
            _remaining = _duration;
            _running = true;
        }

        private void Update()
        {
            if (!_running) return;

            _remaining -= Time.deltaTime;

            if (_remaining <= 0f)
            {
                _remaining = 0f;
                _running = false;
                OnTimerEnd?.Invoke();
            }
        }
        #endregion

        #region Public Functions
        public void Stop() => _running = false;
        #endregion
    }
}