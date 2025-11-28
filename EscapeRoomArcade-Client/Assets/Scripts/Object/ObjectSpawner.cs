using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts.Object
{
    public class ObjectSpawner : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("Spawner configuration defining prefabs and probabilities.")]
        private SpawnerConfig _config;

        [SerializeField, Tooltip("Collider defining the spawn area.")]
        private BoxCollider2D _area;

        private readonly List<GameObject> _spawned = new List<GameObject>();
        #endregion

        #region Monobehaviour Functions
        private void Start()
        {
            StartCoroutine(FillToMaxCoroutine());
        }
        #endregion

        #region Private Functions
        private IEnumerator FillToMaxCoroutine()
        {
            int attempts = 0;
            int maxAttempts = _config.MaxObjects * _config.SpawnAttempts;

            while (_spawned.Count < _config.MaxObjects && attempts < maxAttempts)
            {
                TrySpawnObject();
                attempts++;
                yield return null; // cede un frame a Unity
            }

            if (_spawned.Count < _config.MaxObjects)
                Debug.LogWarning("No se pudieron spawnear todos los objetos: área muy pequeña o colisiones bloquean.");
        }

        private void TrySpawnObject()
        {
            for (int i = 0; i < _config.SpawnAttempts; i++)
            {
                var pos = GetRandomPosition();
                if (IsSpotFree(pos))
                {
                    var prefab = PickRandomPrefab();
                    var inst = Instantiate(prefab, pos, Quaternion.identity);
                    _spawned.Add(inst);
                    return;
                }
            }
        }

        private GameObject PickRandomPrefab()
        {
            float total = 0f;
            foreach (var e in _config.Entries)
                total += e.Weight;

            float r = Random.value * total;
            float acc = 0f;

            foreach (var e in _config.Entries)
            {
                acc += e.Weight;
                if (r <= acc)
                    return e.Prefab;
            }

            return _config.Entries[0].Prefab;
        }

        private Vector2 GetRandomPosition()
        {
            Vector2 size = _area.size;
            Vector2 center = (Vector2)_area.transform.position + _area.offset;

            float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
            float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);

            return new Vector2(x, y);
        }

        private bool IsSpotFree(Vector2 pos)
        {
            var hits = Physics2D.OverlapCircleAll(pos, 0.4f, LayerMask.NameToLayer("Pushable"));
            return hits.Length == 0;
        }
        #endregion

        #region Public Functions
        public void NotifyObjectRemoved(GameObject obj)
        {
            if (_spawned.Contains(obj))
                _spawned.Remove(obj);

            TrySpawnObject();
        }
        #endregion
    }
}