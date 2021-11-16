using System;
using System.Collections.Generic;

namespace Basement.OEPFramework.Futures.Util
{
    public class FutureScenario : IFutureContainer
    {
        private readonly List<CompositeFuture> _compositeFutures = new List<CompositeFuture>();
        private CompositeFuture _current;
        public event Action<bool> onComplete;
        public bool isRun { get; private set; }
        public bool isCancelled { get; private set; }
        public bool isEmpty => _compositeFutures.Count == 1 && _compositeFutures[0].futuresCount == 0;

        public FutureScenario()
        {
            Init();
        }

        private void CompleteFuture(IFuture future)
        {
            IFuture nextFuture = null;
            _compositeFutures.RemoveAt(0);
            if (_compositeFutures.Count > 0)
                nextFuture = _compositeFutures[0];

            if (nextFuture == null)
                Complete();
            else
                nextFuture.Run();
        }

        private void Complete()
        {
            _current = null;
            isRun = false;
            Init();

            onComplete?.Invoke(isCancelled);
        }
        
        private void Init()
        {
            _compositeFutures.Add(new CompositeFuture());
            _current = _compositeFutures[0];
            _current.AddListener(FutureCompletionState.Both, CompleteFuture);
        }
        
        public void Next()
        {
            if (_current.futuresCount == 0) return;
            var newFuture = new CompositeFuture();

            _compositeFutures.Add(newFuture);
            newFuture.AddListener(FutureCompletionState.Both, CompleteFuture);
            _current = newFuture;
        }

        public void Run()
        {
            if (isRun || _compositeFutures[0].futuresCount == 0) return;
            isRun = true;
            isCancelled = false;
            _compositeFutures[0].Run();
        }

        public void AddFuture(IFuture future)
        {
            if (future.isRun || future.isCancelled || future.isDone)
                throw new Exception("future already run or completed");

            _current.AddFuture(future);
        }

        public void ExecuteTask(Action method)
        {
            AddFuture(new FutureTask(method));
        }

        public void Cancel()
        {
            if (isCancelled) return;
            isCancelled = true;
            var cpy = new List<CompositeFuture>(_compositeFutures);
            _compositeFutures.Clear();

            foreach (var f in cpy)
            {
                f.RemoveListener(CompleteFuture);
                f.Cancel();
            }

            Complete();
        }
    }
}
