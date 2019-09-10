namespace Basement.Common.Pool
{
    interface IPooled
    {
        void ToInitialState();
        void Release();
        int GetHashCode();
    }
}
