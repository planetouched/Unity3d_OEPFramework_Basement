using System;
using System.Collections.Generic;
using System.Threading;
using Basement.OEPFramework.Futures;

namespace Basement.Common.Thread
{
    public class FixedThreadsExecutor : IExecutor
    {
        readonly List<IExecutor> threads = new List<IExecutor>();

        public int taskCount
        {
            get
            {
                int sum = 0;

                foreach (var thread in threads)
                    sum += thread.taskCount;

                return sum;
            }
        }

        public FixedThreadsExecutor(int threadsCount, ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            for (int i = 0; i < threadsCount; i++)
            {
                var tw = new SingleThreadExecutor(threadPriority);
                threads.Add(tw);
            }
        }

        IExecutor GetThread()
        {
            int count = threads[0].taskCount;

            if (threads.Count == 1 || count == 0)
                return threads[0];

            int idx = 0;
            for (int i = 1; i < threads.Count; i++)
            {
                int test = threads[i].taskCount;

                if (test == 0)
                    return threads[i];

                if (count > test)
                {
                    count = test;
                    idx = i;
                }
            }

            return threads[idx];
        }

        public void Shutdown()
        {
            var copy = new List<IExecutor>(threads);
            foreach (var thread in copy)
            {
                thread.Shutdown();
            }
        }

        public T Execute<T>(T future) where T : IFuture
        {
            return GetThread().Execute(future);
        }

        public IFuture Execute(Action action)
        {
            return GetThread().Execute(action);
        }

        public IFuture Execute<T>(Func<T> func)
        {
            return GetThread().Execute(func);
        }

        public void Join()
        {
            foreach (var thread in threads)
                thread.Join();
        }
    }
}
