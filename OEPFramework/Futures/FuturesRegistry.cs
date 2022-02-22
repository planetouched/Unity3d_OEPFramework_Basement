using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;

public static class FuturesRegistry
{

    public static event Action<IFuture, IFuture> OnFutureRun;
    public static event Action<IFuture> OnFutureComplete;

    private static readonly Stack<IFuture> s_parentsChain = new Stack<IFuture>();

    public static IDisposable Parent(IFuture parent)
    {
        return new ParentDisposable(parent);
    }

    public static IFuture Run(this IFuture future, IFuture parent)
    {
        using (Parent(parent))
        {
            return future.Run();
        }
    }

    public static void Run(IFuture future)
    {
        var parent = s_parentsChain.Count == 0 ? null : s_parentsChain.Peek();
        OnFutureRun?.Invoke(future, parent);
    }

    public static void Complete(IFuture future)
    {
        OnFutureComplete?.Invoke(future);
    }

    private static void PushParent(IFuture parent)
    {
        s_parentsChain.Push(parent);
    }

    private static void PopParent(IFuture parent)
    {
        if (s_parentsChain.Count == 0)
            throw new Exception($"Trying to pop parent {parent} before push");

        var oldParent = s_parentsChain.Pop();
        
        if (oldParent != parent)
            throw new Exception($"parents push/pop mismatch. Current = {parent}, last = {s_parentsChain.Peek()}");
    }


    private class ParentDisposable : IDisposable
    {
        private IFuture _parent;

        public ParentDisposable(IFuture parent)
        {
            _parent = parent;
            PushParent(_parent);
        }

        public void Dispose()
        {
            PopParent(_parent);
        }
    }
}
