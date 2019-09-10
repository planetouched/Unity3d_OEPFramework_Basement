namespace Basement.OEPFramework.UnityEngine.Behaviour
{
    public interface IControllable : IPlayable
    {
        bool initialized { get; }
        void Initialize();
        void Uninitialize();
    }
}
