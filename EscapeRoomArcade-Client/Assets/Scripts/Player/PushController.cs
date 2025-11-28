using Assets.Scripts.Object;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PushController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private float _basePushForce = 12f;
        [SerializeField] private float _sideTorqueForce = 6f;

        private PlayerController _player;
        #endregion

        #region Monobehaviour Functions
        private void Awake()
        {
            _player = GetComponent<PlayerController>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var pushable = collision.collider.GetComponent<PushableObject>();
            if (pushable == null) return;

            var rb = pushable.Rigidbody;

            Vector2 hitPoint = collision.GetContact(0).point;
            Vector2 center = rb.worldCenterOfMass;

            Vector2 pushDir = _player.LastMoveDirection.normalized;
            float speedFactor = Mathf.Clamp01(_player.CurrentVelocity.magnitude / 10f);

            float forceMagnitude = _basePushForce * (1f / pushable.Weight) * (0.5f + speedFactor);

            rb.AddForce(pushDir * forceMagnitude, ForceMode2D.Impulse);

            Vector2 directionToHit = hitPoint - center;
            float torque = Vector3.Cross(pushDir, directionToHit).z;

            rb.AddTorque(torque * _sideTorqueForce, ForceMode2D.Impulse);

            pushable.OnPushed();
        }
        #endregion
    }
}