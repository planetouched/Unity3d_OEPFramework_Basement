using System;
using System.Collections.Generic;

namespace Basement.OEPFramework.UnityEngine
{
    public static class Sync
    {
        static readonly List<List<Action>> calls = new List<List<Action>>();
        static readonly List<int> callsCount = new List<int>();
        static readonly object syncRoot = new object();

        static Sync()
        {
            //640K Ought to be Enough for Anyone
            for (int i = 0; i < 64; i++)
            {
                calls.Add(new List<Action>());
                callsCount.Add(0);
            }
        }

        public static void Add(Action method, int loopType)
        {
            lock (syncRoot)
            {
                calls[loopType].Add(method);
                callsCount[loopType]++;
            }
        }

        public static void Process(int loopType)
        {
            if (callsCount[loopType] == 0) return;

            List<Action> list;
            lock (syncRoot)
            {
                list = new List<Action>(calls[loopType]);
                callsCount[loopType] = 0;
                calls[loopType].Clear();
            }

            foreach (var element in list)
            {
                element();
            }
        }
    }
}
