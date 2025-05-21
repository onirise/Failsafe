using UnityEngine;

/// <summary>
/// Обнаружение препятствий
/// </summary>
public class ObstacleDetector
{
    private Vector3 _topDownRayOrigin = new(0, 3, 1.5f);
    private Vector3 _forwardRayOrigin = new(0, -0.1f, 0.5f);
    private float _topDownRayDistance = 3f;
    private float _viewAngleThreashold = 20f;
    private float _grabPointDistanceThreashold = 1f;
    private Transform _playerTransform;
    private Transform _headTransform;
    private Transform _playerGrabPoint;
    private ObstacleData _obstacleData;
    public ObstacleData Obstacle => _obstacleData;

    public ObstacleDetector(Transform playerTransform, Transform headTransform, Transform playerGrabPoint)
    {
        _playerTransform = playerTransform;
        _headTransform = headTransform;
        _playerGrabPoint = playerGrabPoint;
    }

    public void HandleFindingObstacle()
    {
        var topDownRayOrigin = _playerTransform.position + _playerTransform.rotation * _topDownRayOrigin;
        _obstacleData = DetectObstacleWithRays(topDownRayOrigin);
        Debug.DrawRay(topDownRayOrigin, Vector3.down * _topDownRayOrigin.z, Color.red);
    }

    public ObstacleData FindObstacleOnDirection(Vector3 direction)
    {
        var topDownRayOrigin = _playerTransform.position + _playerTransform.rotation * _topDownRayOrigin + direction;
        return DetectObstacleWithRays(topDownRayOrigin);
    }

    // TODO Исправить обнаружение препятствий, сейчас тяжело зацепляться за узкие уступы и есть баги когда несколько коллайдеров пересечено:
    // Возможно лучший алгоритм:
    // 1) Кастить сферу из камеры игрока
    // 2) В точку пересечения кастить лучи на боковую и верхнюю грани для нахождения нормалей (как сейчас)
    // 3) Далее как сейчас
    private ObstacleData DetectObstacleWithRays(Vector3 topDownRayOrigin)
    {
        var obstacleData = new ObstacleData();
        if (!Physics.Raycast(topDownRayOrigin, Vector3.down, out obstacleData.TopRaycastHit, _topDownRayDistance))
        {
            return obstacleData;
        }

        //Верхняя грань должна быть параллельна земле
        var dotProduct = Vector3.Dot(obstacleData.TopRaycastHit.normal, Vector3.up);
        if (dotProduct != 1)
        {
            return obstacleData;
        }

        var topDownRayLength = topDownRayOrigin.y - obstacleData.TopRaycastHit.point.y;
        obstacleData.Height = obstacleData.TopRaycastHit.point.y - _playerTransform.position.y;

        var forwardRayOrigin = _playerTransform.position + _playerTransform.rotation * new Vector3(_forwardRayOrigin.x, _forwardRayOrigin.y + obstacleData.Height, _forwardRayOrigin.z);
        if (!Physics.Raycast(forwardRayOrigin, _playerTransform.forward, out obstacleData.FrontRaycastHit, _topDownRayDistance))
        {
            return obstacleData;
        }

        obstacleData.IsFound = true;

        Debug.DrawRay(topDownRayOrigin, Vector3.down * topDownRayLength, Color.red);

        var forwardRayLength = Vector3.Distance(forwardRayOrigin, obstacleData.FrontRaycastHit.point);
        obstacleData.Distance = forwardRayLength;
        obstacleData.FrontSideNormal = obstacleData.FrontRaycastHit.normal;

        Debug.DrawRay(forwardRayOrigin, _playerTransform.forward * forwardRayLength, Color.red);
        Debug.DrawRay(obstacleData.FrontRaycastHit.point, obstacleData.FrontSideNormal, Color.green);

        var directionFromHeadToObstacle = obstacleData.TopRaycastHit.point - _headTransform.position;
        var viewAngle = Vector3.Angle(directionFromHeadToObstacle, _headTransform.forward);
        if (viewAngle <= _viewAngleThreashold)
        {
            obstacleData.InPlayerView = true;
        }

        var distanceToGrabPoint = Vector3.Distance(obstacleData.TopRaycastHit.point, _playerGrabPoint.position);
        if (distanceToGrabPoint <= _grabPointDistanceThreashold)
        {
            obstacleData.AroundGrabPoint = true;
        }

        Debug.DrawLine(_playerGrabPoint.position, obstacleData.TopRaycastHit.point, Color.yellow);
        return obstacleData;
    }
}
/// <summary>
/// Данные препятствия / уступа
/// </summary>
public struct ObstacleData
{
    public bool IsFound;
    public float Height;
    public float Distance;
    public bool InPlayerView;
    public bool AroundGrabPoint;
    public RaycastHit TopRaycastHit;
    public RaycastHit FrontRaycastHit;
    public Vector3 FrontSideNormal;
}