using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Basement.BLFramework.Core.Model
{
    public abstract class ReferenceModelBase<TCategories, TDescription> : ModelBase, IReferenceModel
        where TDescription : IDescription
        where TCategories : class
    {
        public TDescription description { get; }
        public TCategories categories { get; protected set; }

        public bool selectable { get; }
        protected readonly RawNode initNode;

        protected ReferenceModelBase(RawNode initNode, TCategories categories, TDescription description, IContext context) : base(description.key, context)
        {
            this.initNode = initNode;
            this.description = description;
            this.categories = categories;
            selectable = description.selectable;
        }
    }
}