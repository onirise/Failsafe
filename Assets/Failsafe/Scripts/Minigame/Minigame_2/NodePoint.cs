public class NodePoint : BasePoint
{
    public override int GetTotalValue()
    {
        int sum = Value;
        foreach (var conn in Connections)
        {
            sum += conn.GetTotalValue();
        }
        return sum;
    }
}
