namespace Basement.OEPFramework.UnityEngine.Loop.TimeData
{
    public interface ITimeData
    {
        float deltaTime { get; set; }
        float fixedDeltaTime { get; set; }
        float time { get; set; }
        float fixedTime { get; set; }
    }
}