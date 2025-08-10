using Failsafe.Items;
using UnityEngine;

public class ShootAction : IActionWithItem
{
    private readonly Transform _cameraTransform;

    public ShootAction(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
    }

    public ItemUseResult Execute(PlayerHandsContainer playerHandsContainer)
    {
        // Нужно активировать риг для руки и направлять руку по направлению камеры
        Debug.Log("Выстрел");
        var itemInHand = playerHandsContainer.ItemInHand;
        var useResult = itemInHand.ItemUsable?.Use() ?? ItemUseResult.Consumed;
        itemInHand.ItemObject.Use();
        return useResult;
    }
}
