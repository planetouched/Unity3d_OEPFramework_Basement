namespace Basement.BLFramework.Essential.Amounts
{
    public interface IAmount
    {
        string type { get; }
        int Number();
    }
}
