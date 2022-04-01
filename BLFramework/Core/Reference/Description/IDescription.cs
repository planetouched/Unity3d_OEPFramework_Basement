using Basement.BLFramework.Core.Common;
using Basement.BLFramework.Core.Context;
using Basement.Common;

namespace Basement.BLFramework.Core.Reference.Description
{
    public interface IDescription : IHasContext, IChildren<IDescription>, IParent<IDescription>
    {
        bool selectable { get; }
        string key { get; }
        RawNode GetNode();
        string GetDescriptionPath();
        void Initialization();
        IDescription Build(string collectionKey);
    }
}
