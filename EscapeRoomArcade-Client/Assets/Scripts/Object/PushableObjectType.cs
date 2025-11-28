using UnityEngine;

namespace Assets.Scripts.Object
{
    [CreateAssetMenu(fileName = "PushableObjectType", menuName = "Game/Pushable Object Type")]
    public class PushableObjectType : ScriptableObject
    {
        [SerializeField, Tooltip("Physical weight of the object. Higher = harder to push.")]
        private float _weight = 1f;

        [SerializeField, Tooltip("Extra bounce when pushed.")]
        private float _bounciness = 0.1f;

        [SerializeField, Tooltip("Maximum allowed speed.")]
        private float _maxSpeed = 10f;

        public float Weight => _weight;
        public float Bounciness => _bounciness;
        public float MaxSpeed => _maxSpeed;
    }
}