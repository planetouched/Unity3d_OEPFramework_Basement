using System;
using System.Collections.Generic;
using Basement.BLFramework.Core.Common;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.ThroughEvent;

namespace Basement.BLFramework.Core.Model
{
    public interface IModel : IEventSource, ISerialize, IHasContext, IChildren<IModel>, IParent<IModel>
    {
        event Action<IModel> onDestroy;
        string key { get; set; }
        void Initialization();
        IList<IModel> GetModelPath(bool check);
        bool CheckAvailable();
        void Destroy();
    }
}
