using Basement.BLFramework.Core.Reference.Description;

namespace Basement.BLFramework.Core.Model
{
    public interface IReferenceCollection : IModel
    {
        IDescription dataSource { get; }
    }
}