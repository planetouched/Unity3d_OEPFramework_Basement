namespace Basement.OEPFramework.UnityEngine.Loop.TimeData
{
    public class TimeData : ITimeData
    {
        public float deltaTime { get; set; }
        public float fixedDeltaTime { get; set; }
        public float time { get; set; }
        public float fixedTime { get; set; }
    }
}