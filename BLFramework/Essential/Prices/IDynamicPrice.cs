using Basement.BLFramework.Core.ThroughEvent;

namespace Basement.BLFramework.Essential.Prices
{
    public interface IDynamicPrice
    {
        string type { get; }
        
        ModelsPath GetPath();
        
        bool Check(int amount);
        
        void Pay(int amount);
    }
}