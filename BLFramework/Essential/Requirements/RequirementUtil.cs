using System.Collections.Generic;

namespace Basement.BLFramework.Essential.Requirements
{
    public static class RequirementUtil
    {
        public static IRequirement[] Decomposite(IRequirement requirement, List<IRequirement> requirements = null)
        {
            if (requirements == null)
                requirements = new List<IRequirement>();

            if (requirement is WrappedRequirement wr)
                Decomposite(wr.innerRequirement, requirements);
            else
            {
                if (requirement is CompositeRequirement cr)
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