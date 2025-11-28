using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private float _moveSpeed = 8f;
        [SerializeField] private float _acceleration = 14f;
        [SerializeField] private float _deceleration = 20f;

        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private Vector2 _currentVelocity;
        private Vector2 _lastMoveDirection = Vector2.right;

        private bool _inputEnabled = true;
        #endregion

        #region Properties
        public Vector2 LastMoveDirection => _lastMoveDirection;
        public Vector2 CurrentVelocity => _currentVelocity;
        #endregion

        #region Monobehaviour Functions
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!_inputEnabled)
            {
                _moveInput = Vector2.zero;
                return;
            }

            _moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;

            if (_moveInput.sqrMagnitude > 0.01f)
                _lastMoveDirection = _moveInput;
        }

        private void FixedUpdate()
        {
            Vector2 targetVelocity = _moveInput.sqrMagnitude > 0.01f
                ? _moveInput * _moveSpeed
                : Vector2.zero;

            _currentVelocity = Vector2.MoveTowards(
                _currentVelocity,
                targetVelocity,
                (_moveInput.sqrMagnitude > 0.01f ? _acceleration : _deceleration) * Time.fixedDeltaTime
            );

            _rb.velocity = _currentVelocity;
        }
        #endregion

        #region Public Functions
        public void DisableInput()
        {
            _inputEnabled = false;
            _moveInput = Vector2.zero;
            _currentVelocity = Vector2.zero;

            if (_rb != null)
                _rb.velocity = Vector2.zero;
        }

        public void EnableInput()
        {
            _inputEnabled = true;
        }

        #endregion
    }
}
