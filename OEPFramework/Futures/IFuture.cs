using System;

namespace Basement.OEPFramework.Futures
{
    public interface IFuture
    {
        bool isCancelled { get; }
        bool isDone { get; }
        bool isRun { get; }
        bool isExternal { get; }
        bool isPromise { get; }
        IFuture AddListenerOnRun(Action<IFuture> method);
        IFuture AddListener(FutureCompletionState state, Action<IFuture> method);
        IFuture AddListenerOnFinalize(Action<IFuture> method);
        void RemoveListenerOnRun(Action<IFuture> method);
        void RemoveListener(Action<IFuture> method);
        void RemoveListenerOnFinalize(Action<IFuture> method);
        void Complete(bool external = false);
        void Cancel();
        IFuture Run();
        T Cast<T>() where T : IFuture;
        bool Reuse();
    }
}
