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
        public bool dropped { get; private set; }
        public event Action<IDroppableItem> onDrop;

        public virtual void Drop()
        {
            if (dropped) return;
            dropped = true;

            onDrop?.Invoke(this);
            onDrop = null;
        }

        public virtual bool Reuse()
        {
            if (!dropped) return false;
            
            dropped = false;
            return true;
        }
    }
}
