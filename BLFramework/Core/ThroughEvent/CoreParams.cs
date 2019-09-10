using Basement.BLFramework.Core.Context;

namespace Basement.BLFramework.Core.ThroughEvent
{
    public struct CoreParams
    {
        public IContext context;
        public ModelsPath stack;
        public EventCategory category;
    }
}
