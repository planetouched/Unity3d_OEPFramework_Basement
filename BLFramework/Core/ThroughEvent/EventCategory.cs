namespace Basement.BLFramework.Core.ThroughEvent
{
    public class EventCategory
    {
        public string description { get; }
        
        private static int _id;
        private readonly int _hashCode;

        public EventCategory(string description = null)
        {
            _hashCode = _id++;
            this.description = description;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
}
