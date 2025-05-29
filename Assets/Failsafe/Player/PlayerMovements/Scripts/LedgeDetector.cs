using UnityEngine;

/// <summary>
/// Обнаружение препятствий
/// </summary>
public class LedgeDetector
{
    private float _ledgeFindDistance = 3f;
    private float _forwardSpereRadius = 0.2f;
    private float _topDownRayDistance = 3f;
    private float _forwardRayDistance = 2f;
    private float _viewAngleThreashold = 20f;
    private Transform _playerTransform;
    private Transform _headTransform;
    private Transform _playerGrabPoint;
    private float _grabPointDistance = 1f;
    private float _grabPointRadius = 0.2f;
    private float _grabPointDistanceFromEdge = 0.15f;
    private LayerMask _ledgeLayer;
    private LedgeData _ledgeDataInView;
    public LedgeData LedgeInView => _ledgeDataInView;

    public LedgeDetector(Transform playerTransform, Transform headTransform, Transform playerGrabPoint)
    {
        _playerTransform = playerTransform;
        _headTransform = headTransform;
        _playerGrabPoint = playerGrabPoint;
        _ledgeLayer = ~LayerMask.NameToLayer("Ledge");
    }

    public void HandleFindingLedge()
    {
        _ledgeDataInView = DetectLedgeFromViewDirection();
    }

    /// <summary>
    /// Обнаружить выступ перед игроком
    /// </summary>
    /// <returns></returns>
    public LedgeData FindLedgeInFront()
    {
        var topDownRayOrigin = _playerGrabPoint.position;
        topDownRayOrigin.y += _grabPointRadius;
        Debug.DrawRay(topDownRayOrigin, Vector3.down * _topDownRayDistance, Color.red);
        return DetectLedgeWithRays(topDownRayOrigin);
    }
    /// <summary>
    /// Обнаружить выступ в заданном направлении
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public LedgeData FindLedgeOnDirection(Vector3 direction)
    {
        var topDownRayOrigin = _playerGrabPoint.position;
        topDownRayOrigin.y += _grabPointRadius;
        return DetectLedgeWithRays(topDownRayOrigin + direction);
    }

    private LedgeData DetectLedgeFromViewDirection()
    {
        var ledgeData = new LedgeData();
        Debug.DrawRay(_headTransform.position, _headTransform.forward * _ledgeFindDistance, Color.white);
        if (!Physics.SphereCast(_headTransform.position, 0.2f, _headTransform.forward, out var climbingHitInfo, _ledgeFindDistance, _ledgeLayer))
        {
            return ledgeData;
        }
        if (Physics.SphereCast(_headTransform.position, 0.2f, _headTransform.forward, out var viewHitInfo, _ledgeFindDistance))
        {
            if (viewHitInfo.collider.gameObject != climbingHitInfo.collider.gameObject)
            {
                Debug.DrawLine(_headTransform.position, climbingHitInfo.point, Color.green);
                Debug.DrawLine(_headTransform.position, viewHitInfo.point, Color.grey);
                return ledgeData;
            }
        }
        ledgeData.IsFound = true;
        ledgeData.InPlayerView = true;
        Debug.DrawLine(_headTransform.position, climbingHitInfo.point, Color.green);
        Debug.DrawRay(climbingHitInfo.point, climbingHitInfo.normal, Color.cyan);

        ledgeData.DotProduct = Vector3.Dot(climbingHitInfo.normal, Vector3.up);
        if (ledgeData.LookAtFace == LedgeData.LedgeFace.Side)
        {
            ledgeData.FrontSideNormal = climbingHitInfo.normal;
            var topDownRayOrigin = climbingHitInfo.point - ledgeData.FrontSideNormal * _grabPointDistanceFromEdge;
            topDownRayOrigin.y = _playerGrabPoint.position.y + _grabPointRadius;
            if (!Physics.Raycast(topDownRayOrigin, Vector3.down, out var topRaycastHit, _topDownRayDistance))
            {
                return ledgeData;
            }
            ledgeData.Height = topRaycastHit.point.y - _playerTransform.position.y;
            ledgeData.GrabPointPosition = topRaycastHit.point;
        }
        else if (ledgeData.LookAtFace == LedgeData.LedgeFace.BottomEdge)
        {
            ledgeData.FrontSideNormal = Vector3.ProjectOnPlane(climbingHitInfo.normal, Vector3.up).normalized;
            var topDownRayOrigin = climbingHitInfo.point - ledgeData.FrontSideNormal * _grabPointDistanceFromEdge;
            topDownRayOrigin.y = _playerGrabPoint.position.y + _grabPointRadius;
            if (!Physics.Raycast(topDownRayOrigin, Vector3.down, out var topRaycastHit, _topDownRayDistance))
            {
                return ledgeData;
            }
            ledgeData.Height = topRaycastHit.point.y - _playerTransform.position.y;
            ledgeData.GrabPointPosition = topRaycastHit.point;
        }
        else if (ledgeData.LookAtFace == LedgeData.LedgeFace.Top || ledgeData.LookAtFace == LedgeData.LedgeFace.TopEdge)
        {
            var forwardRayOrigin = _playerTransform.position;
            forwardRayOrigin.y = climbingHitInfo.point.y - _grabPointDistanceFromEdge;
            if (!Physics.SphereCast(forwardRayOrigin, _forwardSpereRadius, _playerTransform.forward, out var frontRaycastHit, _forwardRayDistance))
            {
                return ledgeData;
            }
            ledgeData.FrontSideNormal = frontRaycastHit.normal;
            ledgeData.Height = climbingHitInfo.point.y - _playerTransform.position.y;
            ledgeData.GrabPointPosition = frontRaycastHit.point - frontRaycastHit.normal * _grabPointDistanceFromEdge;
            ledgeData.GrabPointPosition.y = climbingHitInfo.point.y;
        }
        else
        {
            return ledgeData;
        }

        var distanceToGrabPoint = Vector3.Distance(ledgeData.GrabPointPosition, _playerGrabPoint.position);
        if (distanceToGrabPoint <= _grabPointDistance)
        {
            ledgeData.AroundGrabPoint = true;
        }

        Debug.DrawLine(_headTransform.position, ledgeData.GrabPointPosition, Color.red);

        return ledgeData;
    }

    private LedgeData DetectLedgeWithRays(Vector3 topDownRayOrigin)
    {
        var ledgeData = new LedgeData();
        if (!Physics.Raycast(topDownRayOrigin, Vector3.down, out var topRaycastHit, _topDownRayDistance, _ledgeLayer))
        {
            return ledgeData;
        }

        //Верхняя грань должна быть параллельна земле
        var dotProduct = Vector3.Dot(topRaycastHit.normal, Vector3.up);
        if (dotProduct != 1)
        {
            return ledgeData;
        }

        var topDownRayLength = topDownRayOrigin.y - topRaycastHit.point.y;
        ledgeData.Height = topRaycastHit.point.y - _playerTransform.position.y;

        var forwardRayOrigin = _playerTransform.position + Vector3.up * (ledgeData.Height - _grabPointDistanceFromEdge);
        if (!Physics.SphereCast(forwardRayOrigin, _forwardSpereRadius, _playerTransform.forward, out var frontRaycastHit, _topDownRayDistance, _ledgeLayer))
        {
            return ledgeData;
        }

        ledgeData.IsFound = true;

        Debug.DrawRay(topDownRayOrigin, Vector3.down * topDownRayLength, Color.red);

        var forwardRayLength = Vector3.Distance(forwardRayOrigin, frontRaycastHit.point);
        ledgeData.Distance = forwardRayLength;
        ledgeData.FrontSideNormal = frontRaycastHit.normal;

        Debug.DrawRay(forwardRayOrigin, _playerTransform.forward * forwardRayLength, Color.red);
        Debug.DrawRay(frontRaycastHit.point, ledgeData.FrontSideNormal, Color.green);

        var directionFromHeadToLedge = topRaycastHit.point - _headTransform.position;
        var viewAngle = Vector3.Angle(directionFromHeadToLedge, _headTransform.forward);
        if (viewAngle <= _viewAngleThreashold)
        {
            ledgeData.InPlayerView = true;
        }

        var distanceToGrabPoint = Vector3.Distance(topRaycastHit.point, _playerGrabPoint.position);
        if (distanceToGrabPoint <= _grabPointDistance)
        {
            ledgeData.AroundGrabPoint = true;
        }

        ledgeData.GrabPointPosition = topRaycastHit.point;
        return ledgeData;
    }
}
/// <summary>
/// Данные выступа
/// </summary>
public struct LedgeData
{
    public enum LedgeFace { Top, Bottom, Side, TopEdge, BottomEdge }
    /// <summary>
    /// Выступ найден
    /// </summary>
    public bool IsFound;
    /// <summary>
    /// Игрок смотрит на выступ
    /// </summary>
    public bool InPlayerView;
    public float DotProduct;
    /// <summary>
    /// Грань, на которую смотрит игрок
    /// </summary>
    public LedgeFace LookAtFace => DotProduct switch
    {
        1 => LedgeFace.Top,
        -1 => LedgeFace.Bottom,
        0 => LedgeFace.Side,
        > 0 => LedgeFace.TopEdge,
        < 0 => LedgeFace.BottomEdge,
        _ => throw new System.NotImplementedException(),
    };
    /// <summary>
    /// Высота выступа, считается от текущей позиции игрока
    /// </summary>
    public float Height;
    /// <summary>
    /// Расстояние от игрока до боковой грани 
    /// </summary>
    public float Distance;
    /// <summary>
    /// Точка на выступе за которую можно зацепистья
    /// </summary>
    public Vector3 GrabPointPosition;
    /// <summary>
    /// Игрок в радиусе зацепления
    /// </summary>
    public bool AroundGrabPoint;
    public RaycastHit TopRaycastHit;
    public RaycastHit FrontRaycastHit;
    /// <summary>
    /// Нормаль к боковой грани выступа
    /// </summary>
    public Vector3 FrontSideNormal;


    public override string ToString()
    {
        return $"DotProduct: {DotProduct}, LookAtFace: {LookAtFace}, GrabPointPosition: {GrabPointPosition}, Height: {Height}, Distance: {Distance}";
    }
}