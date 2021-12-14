using System.Collections.Generic;
using Basement.OEPFramework.UnityEngine.Behaviour;

namespace Basement.OEPFramework.UnityEngine.Loop
{
    public class EngineLoop
    {
        private class InnerComparer : IComparer<LoopBehaviour>
        {
            private readonly int _loopType;

            public InnerComparer(int loopType)
            {
                _loopType = loopType;
            }

            public int Compare(LoopBehaviour x, LoopBehaviour y)
            {
                if (x.GetOrder(_loopType) > y.GetOrder(_loopType)) return 1;
                if (x.GetOrder(_loopType) < y.GetOrder(_loopType)) return -1;
                return 0;
            }
        }

        private readonly InnerComparer _comparer;

        private readonly List<LoopBehaviour> _items = new List<LoopBehaviour>();
        private readonly List<LoopBehaviour> _toAdd = new List<LoopBehaviour>();
        private readonly List<LoopBehaviour> _toRemove = new List<LoopBehaviour>();

        private int _behaviourOrder;
        private readonly int _loopType;

        public EngineLoop(int loopType)
        {
            _comparer = new InnerComparer(loopType);
            _loopType = loopType;
        }

        public void AddToLast(LoopBehaviour behaviour)
        {
            _toAdd.Add(behaviour);
        }

        public void Remove(LoopBehaviour behaviour)
        {
            int idx = _toAdd.IndexOf(behaviour);
            if (idx != -1)
                _toAdd.RemoveAt(idx);
            else
                _toRemove.Add(behaviour);
        }

        public void CallAllBehavioursActions()
        {
            Sync.Process(_loopType);
            
            ModifyIfNeeded();

            InnerCall(_items);

            for (;;)
            {
                var newLoops = ModifyIfNeeded();
                if (newLoops != null)
                {
                    InnerCall(newLoops);
                }
                else
                {
                    break;
                }
            }
        }

        public void Refresh()
        {
            ModifyIfNeeded();
        }

        private void InnerCall(List<LoopBehaviour> loops)
        {
            for (int i = 0; i < loops.Count; i++)
            {
                var current = loops[i];
                if (current != null && !current.dropped && current.callActions)
                    current.ExecuteAction(_loopType);
            }
        }

        private List<LoopBehaviour> ModifyIfNeeded()
        {
            if (_toRemove.Count > 0)
            {
                foreach (var behaviour in _toRemove)
                {
                    int idx = GetIndex(behaviour);
                    if (idx >= 0)
                        _items.RemoveAt(idx);
                }

                _toRemove.Clear();
            }

            if (_toAdd.Count > 0)
            {
                var newLoops = new List<LoopBehaviour>();
                
                foreach (var behaviour in _toAdd)
                {
                    if (behaviour.dropped) continue;
                    
                    _items.Add(behaviour);

                    if (behaviour.callWhenAdded && behaviour.callActions)
                    {
                        newLoops.Add(behaviour);
                    }

                    behaviour.SetOrder(_loopType, _behaviourOrder++);
                }

                _toAdd.Clear();
                return newLoops;
            }

            return null;
        }

        private int GetIndex(LoopBehaviour behaviour)
        {
            return _items.BinarySearch(behaviour, _comparer);
        }
    }
}