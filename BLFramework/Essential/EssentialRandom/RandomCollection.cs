using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.Common;

namespace Basement.BLFramework.Essential.EssentialRandom
{
    public class RandomCollection : ReferenceCollectionBase<Random, RandomCategories, RandomDescription>
    {
        public RandomCollection(RawNode initNode, RandomCategories categories, IContext context, IDescription dataSource) : base(initNode, categories, context, dataSource)
        {
            foreach (var model in this)
            {
            }
        }

        protected override Random Factory(RawNode initNode, RandomDescription description)
        {
            return new Random(initNode, categories, description, GetContext());
        }
    }
}
