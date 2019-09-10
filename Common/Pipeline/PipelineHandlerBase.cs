using System.Threading;

namespace Basement.Common.Pipeline
{
    public abstract class PipelineHandlerBase : IPipelineHandler
    {
        private int _error;
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(true);

        public abstract object ReturnItem();
        public abstract void Create(object data);

        public int GetError()
        {
            return _error;
        }

        public void SetError(int errorCode)
        {
            _error = errorCode;
        }

        public void Sleep()
        {
            _resetEvent.Reset();
            _resetEvent.WaitOne();
        }

        public void Wakeup()
        {
            _resetEvent.Set();
        }
    }
}
