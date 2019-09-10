using System;

namespace Basement.Common
{
    public class Lazy<T>
    {
        public bool isCreated { get; private set; }
        public Action<T> onCreate  { get; set; }

        private T _value;
        private Func<T> _initFunc;
        
        public Lazy(Func<T> initFunc)
        {
            _initFunc = initFunc;
        }

        public void Create()
        {
            GetValue();
        }

        public T GetValue()
        {
            if (isCreated)
                return _value;

            _value = _initFunc();
            _initFunc = null;
            isCreated = true;
            
            if (onCreate != null)
                onCreate(_value);

            return _value;
        }

        public void ClearFactory()
        {
            _initFunc = null;
        }
    }
}
