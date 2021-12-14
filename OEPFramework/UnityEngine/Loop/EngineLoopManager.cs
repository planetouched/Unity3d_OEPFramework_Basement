using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Basement.OEPFramework.UnityEngine.Loop
{
    public static class EngineLoopManager
    {
        private static readonly List<EngineLoop> _loops = new List<EngineLoop>();

        public static int LoopsCount()
        {
            return _loops.Count;
        }

        static bool CheckInit()
        {
#if UNITY_EDITOR
            if (_loops.Count == 0)
            {
                Debug.LogError("Loops are not adjusted. Use Loops.Setup()");
                EditorApplication.isPlaying = false;
                return false;
            }
#endif
            return true;
        }

        public static EngineLoop GetEngineLoop(int loopType)
        {
            return _loops[loopType];
        }

        public static int AddNewEngineLoop()
        {
            int loop = _loops.Count;
            _loops.Add(new EngineLoop(loop));
            return loop;
        }

        public static void Refresh()
        {
            var loops = LoopsCount();
            for (int i = 0; i < loops; i++)
            {
                var loop = _loops[i];
                loop.Refresh();
            }
        }

        public static void Execute(int loopType)
        {
#if UNITY_EDITOR
            if (!CheckInit()) return;
#endif
            _loops[loopType].CallAllBehavioursActions();
        }
    }
}
