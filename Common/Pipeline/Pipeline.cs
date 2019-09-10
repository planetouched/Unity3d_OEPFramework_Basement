using System;
using System.Collections.Generic;

namespace Basement.Common.Pipeline
{
    public class Pipeline
    {
        private class PipelineOption
        {
            private readonly Func<IPipelineHandler> _factory;
            private readonly IPipelineHandler _handler;

            public PipelineOption(Func<IPipelineHandler> factory, IPipelineHandler handler)
            {
                _factory = factory;
                _handler = handler;
            }

            public IPipelineHandler GetHandler()
            {
                if (_factory != null)
                    return _factory();

                return _handler;
            }
        }

        private readonly List<PipelineOption> _pipelineHandlers = new List<PipelineOption>();

        public event Action<IPipelineHandler> onError;
        public event Action<IPipelineHandler> onHandlerPassed;

        public Pipeline AddHandler(Func<IPipelineHandler> handlerFactory)
        {
            _pipelineHandlers.Add(new PipelineOption(handlerFactory, null));
            return this;
        }

        public Pipeline AddHandler(IPipelineHandler handler)
        {
            _pipelineHandlers.Add(new PipelineOption(null, handler));
            return this;
        }

        public Pipeline RemoveHandler(int idx)
        {
            _pipelineHandlers.RemoveAt(idx);
            return this;
        }

        public object Start(object item)
        {
            object currentItem = item;
            foreach (var factory in _pipelineHandlers)
            {
                var handler = factory.GetHandler();
                handler.Create(currentItem);

                var newElement = handler.ReturnItem();
                if (newElement != null)
                    currentItem = newElement;

                if (onHandlerPassed != null)
                    onHandlerPassed(handler);

                if (handler.GetError() != 0)
                {
                    if (onError != null)
                        onError(handler);
                    return null;
                }
            }
            return currentItem;
        }
    }
}
