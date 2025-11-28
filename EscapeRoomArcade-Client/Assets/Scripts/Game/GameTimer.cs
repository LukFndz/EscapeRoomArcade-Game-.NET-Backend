using System;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public sealed class GameTimer : MonoBehaviour
    {
        #region Events
        public event Action OnTimerEnd;
        public event Action OnTimerStop;


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
            _running = false;
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

        public void StopTimer()
        {
            _running = false;
            OnTimerStop?.Invoke();
        }

        public void StartTimer()
        {
            _running = true;
        }

        public void Stop() => _running = false;
        #endregion
    }
}