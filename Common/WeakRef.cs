using System;

namespace Basement.Common
{
    public class WeakRef<T> where T : class
    {
        private readonly WeakReference _reference;

        public T obj => isAlive ? (T)_reference.Target : null;

        public bool isAlive => _reference != null && _reference.IsAlive;

        public bool trackResurrection => _reference.TrackResurrection;

        public WeakRef(T reference)
        {
            _reference = reference == null ? null : new WeakReference(reference);
        }
    }
}
