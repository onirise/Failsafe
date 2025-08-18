using Failsafe.Items;

public class UseOnSelfAction : IActionWithItem
{
    public ItemUseResult Execute(PlayerHandsContainer playerHandsContainer)
    {
        var itemInHand = playerHandsContainer.ItemInHand;
        var useResult = itemInHand.ItemUsable?.Use() ?? ItemUseResult.Consumed;
        itemInHand.ItemObject.Use();

        if (useResult.ItemStateAfterUse == ItemState.Consume)
        {
            playerHandsContainer.SetItemNull();
        }
        return useResult;
    }
}
