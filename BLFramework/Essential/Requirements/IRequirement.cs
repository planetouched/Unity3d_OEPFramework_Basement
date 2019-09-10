using Basement.BLFramework.Core.ThroughEvent;

namespace Basement.BLFramework.Essential.Requirements
{
    public interface IRequirement
    {
        string type { get; }
        ModelsPath GetPath();
        bool Check();
    }
}