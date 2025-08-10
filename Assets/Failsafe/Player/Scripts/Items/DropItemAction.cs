using Failsafe.Items;
using UnityEngine;

public class DropItemAction : IActionWithItem
{
    private float _throwForce = 2;
    private Vector3 _startPosition = new Vector3(0, -1, 1);
    private readonly Transform _cameraTransform;

    public DropItemAction(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }

    public ItemUseResult Execute(PlayerHandsContainer playerHandsContainer)
    {
        var cameraYrotation = Quaternion.Euler(new Vector3(0, _cameraTransform.rotation.eulerAngles.y, 0));
        var direction = cameraYrotation * Vector3.forward;
        var item = playerHandsContainer.DropItemFromHand();
        item.Use();
        item.gameObject.transform.position = _cameraTransform.position + cameraYrotation * _startPosition;
        var itemRb = item.GetComponent<Rigidbody>();
        itemRb.AddForce(direction * _throwForce, ForceMode.Impulse);
        return new ItemUseResult { };
    }
}