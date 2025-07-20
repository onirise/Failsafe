using Failsafe.Player.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Failsafe.Player.Scripts
{
    public class TestPrefabSpowner : MonoBehaviour
    {
        [Tooltip("Используется для создания префаба из скоупа игрока. Только для теста!")]
        [SerializeField] private GameObject _testPrefab;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private PlayerView _playerView;

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                var position = _playerView.transform.position + _playerView.transform.forward + Vector3.up;
                var testObject = _objectResolver.Instantiate(_testPrefab, position, Quaternion.identity);
                Debug.Log($"TestItem {testObject.name} spawned at {position}");
            }
        }
    }
}
