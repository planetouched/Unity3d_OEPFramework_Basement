using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;

namespace Basement.BLFramework.Essential.Choices
{
    public interface IPathChoice
    {
        ModelsPath GetModelPath();
        T GetDescription<T>() where T : class, IDescription;
    }
}
