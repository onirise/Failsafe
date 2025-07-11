public class EndPoint : BasePoint
{
    // Проверка совпадения суммы значения с подключенным объектом
    public bool CheckValueMatch()
    {
        foreach (var conn in Connections)
        {
            if (conn.GetTotalValue() == Value)
                return true;
        }
        return false;
    }
}
