namespace Basement.BLFramework.Core.Common
{
    public interface IParent<T>
    {
        void SetParent(T parent);
        T GetParent();
    }
}