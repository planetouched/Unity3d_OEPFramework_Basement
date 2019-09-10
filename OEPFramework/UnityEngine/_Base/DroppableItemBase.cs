using System;
#if REFVIEW
using Basement.Common.Util;
#endif

namespace Basement.OEPFramework.UnityEngine._Base
{
    public abstract class DroppableItemBase :
#if REFVIEW
        ReferenceCounter,
#endif
        IDroppableItem
    {
        public static int globalHashCode;

        private readonly int _hashCode;
        public bool dropped { get; protected set; }
        public event Action<IDroppableItem> onDrop;

        protected DroppableItemBase()
        {
            _hashCode = globalHashCode++;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public virtual void Drop()
        {
            if (dropped) return;
            dropped = true;

            onDrop?.Invoke(this);
            onDrop = null;
        }
    }
}
