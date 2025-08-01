namespace Failsafe.Items
{
    public enum UsageType { ClickToUse, HoldToUse }
    public enum ItemState { Consume, Drop, Hold }
    /// <summary>
    /// Результат от использования предмета
    /// </summary>
    public struct ItemUseResult
    {
        /// <summary>
        /// Как должен использоваться предмет
        /// </summary>
        public UsageType UsageType;
        /// <summary>
        /// Состояния предмета после использвания
        /// </summary>
        public ItemState ItemStateAfterUse;

        /// <summary>
        /// Расходник, который исчезает после использования
        /// </summary>
        public static ItemUseResult Consumed => new ItemUseResult { UsageType = UsageType.ClickToUse, ItemStateAfterUse = ItemState.Consume };
    }
}
