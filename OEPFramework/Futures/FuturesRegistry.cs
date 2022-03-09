using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;

#if FUTURES_ANALYZER
public static class FuturesRegistry
{

    public static event Action<IFuture, IFuture> OnFutureRun;
    public static event Action<IFuture> OnFutureComplete;

    private static readonly Stack<IFuture> _parentsChain = new Stack<IFuture>();

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
        var parent = _parentsChain.Count == 0 ? null : _parentsChain.Peek();
        OnFutureRun?.Invoke(future, parent);
    }

    public static void Complete(IFuture future)
    {
        OnFutureComplete?.Invoke(future);
    }

    private static void PushParent(IFuture parent)
    {
        _parentsChain.Push(parent);
    }

    private static void PopParent(IFuture parent)
    {
        if (_parentsChain.Count == 0)
        {
            throw new Exception($"Trying to pop parent {parent} before push");
        }

        var oldParent = _parentsChain.Pop();

        if (oldParent != parent)
        {
            throw new Exception($"parents push/pop mismatch. Current = {parent}, last = {_parentsChain.Peek()}");
        }
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

#else

public static class FuturesRegistry
{
    public static event Action<IFuture, IFuture> OnFutureRun;
    public static event Action<IFuture> OnFutureComplete;

    private static readonly IDisposable _fakeDisposable = new FakeDisposable();

    public static IDisposable Parent(IFuture parent)
    {
        return _fakeDisposable;
    }

    public static IFuture Run(this IFuture future, IFuture parent) => future.Run();

    public static void Run(IFuture future) {}

    public static void Complete(IFuture future) {}

    private class FakeDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}

#endif
