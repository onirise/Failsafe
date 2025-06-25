using UnityEngine;

namespace Failsafe.GameSceneServices.SpawnSystem
{
    /// <summary>
    /// Тип точки спауна, разные противники могу спавниться на определенных точках
    /// </summary>
    public enum SpawnPointType
    {
        Default,
        /// <summary>
        /// Вентиляция
        /// </summary>
        Venting
    }
    /// <summary>
    /// Точка, нра которой может заспавниться противник
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        public SpawnPointType Type;
        public bool Enabled => _spawnedAt + _spawnDelay < Time.time;

        [SerializeField] private float _spawnDelay = 5f;
        [SerializeField] private float _spawnedAt = -1000;

        public void EnemySpawned()
        {
            _spawnedAt = Time.time;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Position, 0.5f * Vector3.one);
        }
    }
}