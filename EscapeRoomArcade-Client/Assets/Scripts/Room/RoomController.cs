using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Game;
using Assets.Scripts.Object;

namespace Assets.Scripts.Room
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private GameTimer _timer;
        [SerializeField] private ObjectSpawner _spawner;
        [SerializeField] private PlayerController _player;

        private bool _playerInside = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_playerInside) return;

            _playerInside = true;
            StartRoom();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (!_playerInside) return;

            _playerInside = false;
            StopRoom();
        }

        private void StartRoom()
        {
            _timer.StartTimer();
            _spawner.EnableSpawning();
        }

        private void StopRoom()
        {
            _timer.StopTimer();
            _spawner.DisableSpawning();
        }
    }
}
