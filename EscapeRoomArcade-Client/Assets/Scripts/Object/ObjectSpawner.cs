using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Object
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnerConfig _config;
        [SerializeField] private BoxCollider2D _area;

        private readonly List<GameObject> _spawned = new();
        private bool _active;
        private Coroutine _fillCoroutine;
        private int _pushableMask => LayerMask.GetMask("Pushable");

        private void OnDisable()
        {
            StopFilling();
        }

        public void EnableSpawning()
        {
            if (_active) return;
            _active = true;
            if (_fillCoroutine == null)
                _fillCoroutine = StartCoroutine(FillToMaxCoroutine());
        }

        public void DisableSpawning()
        {
            if (!_active) return;
            _active = false;
            StopFilling();
        }

        private void StopFilling()
        {
            if (_fillCoroutine != null)
            {
                StopCoroutine(_fillCoroutine);
                _fillCoroutine = null;
            }
        }

        private IEnumerator FillToMaxCoroutine()
        {
            int attempts = 0;
            int maxAttempts = _config.MaxObjects * _config.SpawnAttempts;

            while (_active && _spawned.Count < _config.MaxObjects && attempts < maxAttempts)
            {
                TrySpawnObject();
                attempts++;
                yield return null;
            }

            _fillCoroutine = null;
        }

        private void TrySpawnObject()
        {
            if (!_active) return;

            for (int i = 0; i < _config.SpawnAttempts; i++)
            {
                Vector2 pos = GetRandomPosition();
                if (!IsSpotFree(pos)) continue;

                GameObject prefab = PickRandomPrefab();
                GameObject inst = Instantiate(prefab, pos, Quaternion.identity);
                _spawned.Add(inst);
                return;
            }
        }

        private GameObject PickRandomPrefab()
        {
            float total = 0f;
            foreach (var e in _config.Entries) total += e.Weight;

            float r = Random.value * total;
            float acc = 0f;
            foreach (var e in _config.Entries)
            {
                acc += e.Weight;
                if (r <= acc) return e.Prefab;
            }
            return _config.Entries[0].Prefab;
        }

        private Vector2 GetRandomPosition()
        {
            Vector2 size = _area.size;
            Vector2 center = (Vector2)_area.transform.position + _area.offset;
            float x = Random.Range(center.x - size.x * 0.5f, center.x + size.x * 0.5f);
            float y = Random.Range(center.y - size.y * 0.5f, center.y + size.y * 0.5f);
            return new Vector2(x, y);
        }

        private bool IsSpotFree(Vector2 pos)
        {
            return Physics2D.OverlapCircleAll(pos, 0.4f, _pushableMask).Length == 0;
        }

        public void NotifyObjectRemoved(GameObject obj)
        {
            if (_spawned.Remove(obj) && _active)
                TrySpawnObject();
        }
    }
}
