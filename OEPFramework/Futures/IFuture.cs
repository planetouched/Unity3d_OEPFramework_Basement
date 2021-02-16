using System;

namespace Basement.OEPFramework.Futures
{
    public interface IFuture
    {
        bool isCancelled { get; }
        bool isDone { get; }
        bool wasRun { get; }
        IFuture AddListenerOnRun(Action<IFuture> method);
        IFuture AddListener(Action<IFuture> method);
        void RemoveListenerOnRun(Action<IFuture> method);
        void RemoveListener(Action<IFuture> method);
        void Cancel();
        IFuture Run();
        T Cast<T>() where T : IFuture;
        bool Reuse();
    }
}
