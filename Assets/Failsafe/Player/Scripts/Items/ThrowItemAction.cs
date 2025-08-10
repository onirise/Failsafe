using Failsafe.Items;
using UnityEngine;

public class ThrowItemAction : IActionWithItem
{
    private float _throwForce = 10;
    // Начальная точка справа от головы, пока так потому что нет анимации броска
    private Vector3 _startPosition = new Vector3(0.5f, 0, 0);
    private readonly Transform _cameraTransform;

    public ThrowItemAction(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }

    public ItemUseResult Execute(PlayerHandsContainer playerHandsContainer)
    {
        var direction = _cameraTransform.forward;
        var item = playerHandsContainer.DropItemFromHand();
        item.Use();
        item.gameObject.transform.position = _cameraTransform.position + _cameraTransform.rotation * _startPosition;
        var itemRb = item.GetComponent<Rigidbody>();
        itemRb.AddForce(direction * _throwForce, ForceMode.Impulse);
        return new ItemUseResult { };
    }
}
