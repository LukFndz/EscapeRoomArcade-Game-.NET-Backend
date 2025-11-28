using UnityEngine;

namespace Assets.Scripts.Object
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PushableObject : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("Object type defining weight and physics behavior.")]
        private PushableObjectType _type;

        private Rigidbody2D _rb;
        #endregion

        #region Properties
        public float Weight => _type.Weight;
        public Rigidbody2D Rigidbody => _rb;
        public float MaxSpeed => _type.MaxSpeed;
        #endregion

        #region Monobehaviour Functions
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_rb.velocity.magnitude > _type.MaxSpeed)
                _rb.velocity = _rb.velocity.normalized * _type.MaxSpeed;
        }
        #endregion

        #region Public Functions
        public void OnPushed() { }
        #endregion
    }
}
