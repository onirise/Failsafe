using UnityEngine;

public class EnemyMemory
{  
    private Vector3 _lastKnownPlayerPosition;
    private Vector3 _lastKnownPlayerDirection;
    
    public void SetLastKnownPlayerPosition(Vector3 position, Vector3 direction)
    {
        _lastKnownPlayerPosition = position;
        _lastKnownPlayerDirection = direction.normalized;
    }
    
    public Vector3 LastKnownPlayerPosition => _lastKnownPlayerPosition;
    public Vector3 LastKnownPlayerDirection => _lastKnownPlayerDirection;
}
