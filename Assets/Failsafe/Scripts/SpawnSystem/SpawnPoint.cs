using UnityEngine;

namespace SpawnSystem
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
        public SpawnPointType type;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Position, 0.5f * Vector3.one);
        }
    }
}