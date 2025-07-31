using UnityEngine;
using UnityEngine.AI;

public class EnemyMovePatterns
{
    private NavMeshAgent _navMeshAgent;
    
    public EnemyMovePatterns(NavMeshAgent  navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }
    
    public Vector3 RandomPointInForwardCone(Vector3 center, Vector3 forward, float radius, float angle = 90f)
    {
        Vector2 circle = UnityEngine.Random.insideUnitCircle * radius;
        Vector3 randomOffset = new Vector3(circle.x, 0, circle.y);

        // Направление смещения по конусу
        Quaternion rotation = Quaternion.AngleAxis(Random.Range(-angle / 2f, angle / 2f), Vector3.up);
        Vector3 direction = rotation * forward.normalized;
        Vector3 randomPoint = center + direction * Random.Range(radius * 0.5f, radius);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return center; // fallback
    }
    
    public Vector3 RandomPointAround(Vector3 center, float radius)
    {
        Vector2 circle = UnityEngine.Random.insideUnitCircle * radius;
        Vector3 randomPoint = new Vector3(center.x + circle.x, center.y, center.z + circle.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return center; // fallback
    }
}
