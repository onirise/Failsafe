namespace Failsafe.Items
{
    /// <summary>
    /// Предмет, который можно использовать
    /// </summary>
    public interface IUsable
    {
        /// <summary>
        /// Использовать
        /// </summary>
        public ItemUseResult Use();
    }
}
