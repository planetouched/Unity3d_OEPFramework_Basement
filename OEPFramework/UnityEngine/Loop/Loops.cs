namespace Basement.OEPFramework.UnityEngine.Loop
{
    public static partial class Loops
    {
        public static int TIMER;
        public static int FIXED_UPDATE;
        public static int UPDATE;
        public static int LATE_UPDATE;
        public static int LEGACY_GUI;

        public static void Init()
        {
            TIMER = EngineLoopManager.AddNewEngineLoop();
            FIXED_UPDATE = EngineLoopManager.AddNewEngineLoop();
            UPDATE = EngineLoopManager.AddNewEngineLoop();
            LATE_UPDATE = EngineLoopManager.AddNewEngineLoop();
            LEGACY_GUI = EngineLoopManager.AddNewEngineLoop();
        }
    }
}
