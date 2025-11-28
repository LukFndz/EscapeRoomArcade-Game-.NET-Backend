using UnityEngine;

namespace Assets.Scripts.Object
{
    [CreateAssetMenu(fileName = "SpawnerConfig", menuName = "Game/Spawner Config")]
    public class SpawnerConfig : ScriptableObject
    {
        [SerializeField, Tooltip("List of objects that can spawn.")]
        private SpawnableEntry[] _entries;

        [SerializeField, Tooltip("Maximum number of objects allowed simultaneously.")]
        private int _maxObjects = 12;

        [SerializeField, Tooltip("Attempts to find a non-overlapping position.")]
        private int _spawnAttempts = 10;

        public SpawnableEntry[] Entries => _entries;
        public int MaxObjects => _maxObjects;
        public int SpawnAttempts => _spawnAttempts;
    }

    [System.Serializable]
    public class SpawnableEntry
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _weight = 1f; // probability

        public GameObject Prefab => _prefab;
        public float Weight => _weight;
    }
}