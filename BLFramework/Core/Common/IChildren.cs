namespace Basement.BLFramework.Core.Common
{
    public interface IChildren<T>
    {
        T GetChild(string collectionKey);
        void AddChild(T child);
        void RemoveChild(string collectionKey);
        bool Exist(string collectionKey);
    }
}
