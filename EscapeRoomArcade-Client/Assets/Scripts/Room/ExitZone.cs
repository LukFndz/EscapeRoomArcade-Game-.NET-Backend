using Assets.Scripts.Object;
using Assets.Scripts.Score;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public class ExitZone : MonoBehaviour
    {
        [SerializeField] private ObjectSpawner _spawner;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PushableObject pushable))
            {
                int value = pushable.Value;

                ScoreManager.Instance.AddScore(value);

                _spawner.NotifyObjectRemoved(collision.gameObject);

                Destroy(collision.gameObject);
            }
        }
    }
}
