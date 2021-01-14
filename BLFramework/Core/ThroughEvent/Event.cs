using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;

namespace Basement.BLFramework.Core.ThroughEvent
{
    public class Event
    {
        internal class InnerComparer : IComparer<KeyValuePair<int, EventHandler>>
        {
            public int Compare(KeyValuePair<int, EventHandler> x, KeyValuePair<int, EventHandler> y)
            {
                if (x.Key > y.Key) return 1;
                if (x.Key < y.Key) return -1;
                return 0;
            }
        }
        public delegate void EventHandler(CoreParams cp, object args);

        private static readonly InnerComparer _comparer = new InnerComparer();
        private static int _globalAttachId;
        private int _attachId;
        private readonly IDictionary<EventCategory, IList<KeyValuePair<int, EventHandler>>> _handlers = new Dictionary<EventCategory, IList<KeyValuePair<int, EventHandler>>>();

        void InnerAttach(EventCategory category, EventHandler func)
        {
            IList<KeyValuePair<int, EventHandler>> list;
            if (_handlers.TryGetValue(category, out list))
                list.Add(new KeyValuePair<int, EventHandler>(_attachId, func));
            else
            {
                list = new List<KeyValuePair<int, EventHandler>> { new KeyValuePair<int, EventHandler>(_attachId, func) };
                _handlers.Add(category, list);
            }
        }

        public void Attach(EventCategory category, EventHandler func)
        {
            _attachId = _globalAttachId++;
            InnerAttach(category, func);
        }

        void InnerDetach(EventCategory category, EventHandler func)
        {
            if (_handlers.TryGetValue(category, out var list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Value == func)
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void Detach(EventCategory category, EventHandler func)
        {
            InnerDetach(category, func);
        }

        public static void Call(EventCategory category, IList<IModel> models, object args, IContext context)
        {
            var toCall = new List<KeyValuePair<int, EventHandler>>();
            foreach (var model in models)
            {
                if (model.GetEvent()._handlers.TryGetValue(category, out var list))
                    toCall.AddRange(list);
            }

            if (toCall.Count == 0) return;

            if (toCall.Count > 1)
                toCall.Sort(_comparer);

            var eventCallStack = new ModelsPath();
            eventCallStack.Set(models, true);

            CoreParams cp;
            cp.stack = eventCallStack;
            cp.context = context;
            cp.category = category;

            switch (toCall.Count)
            {
                case 1:
                    toCall[0].Value(cp, args);
                    break;
                case 2:
                    {
                        var c0 = toCall[0];
                        var c1 = toCall[1];
                        c0.Value(cp, args);
                        c1.Value(cp, args);
                        break;
                    }
                case 3:
                    {
                        var c0 = toCall[0];
                        var c1 = toCall[1];
                        var c2 = toCall[2];
                        c0.Value(cp, args);
                        c1.Value(cp, args);
                        c2.Value(cp, args);
                        break;
                    }
                default:
                    {
                        var cpy = new KeyValuePair<int, EventHandler>[toCall.Count];
                        toCall.CopyTo(cpy, 0);
                        foreach (var pair in cpy)
                            pair.Value(cp, args);
                        break;
                    }
            }
        }

        public void Clear()
        {
            _handlers.Clear();
        }
    }
}
