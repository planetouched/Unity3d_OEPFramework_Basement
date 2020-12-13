using Basement.BLFramework.Core.Common;
using Basement.BLFramework.Core.Model;
using Basement.Common;

namespace Basement.BLFramework.Core.Context
{
    public interface IContext: IChildren<IModel>
    {
        DataSources dataSources { get; }
        RawNode repositories { get; }
        T GetChild<T>(string collectionKey) where T : class;

        void Destroy();
    }
}
