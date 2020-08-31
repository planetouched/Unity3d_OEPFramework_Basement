using System.Collections.Generic;
using Basement.BLFramework.Essential.Requirements;

namespace Basement.BLFramework.Essential.Rewards
{
    public static class RequirementUtil
    {
        public static IRequirement[] Decomposite(IRequirement requirement, List<IRequirement> requirements = null)
        {
            if (requirements == null)
                requirements = new List<IRequirement>();

            var wr = requirement as WrappedRequirement;

            if (wr != null)
                Decomposite(wr.innerRequirement, requirements);
            else
            {
                var cr = requirement as CompositeRequirement;

                if (cr != null)
                {
                    foreach (var r in cr.requirements)
                        Decomposite(r.Value, requirements);
                }
                else
                    requirements.Add(requirement);
            }

            return requirements.ToArray();
        }
    }
}