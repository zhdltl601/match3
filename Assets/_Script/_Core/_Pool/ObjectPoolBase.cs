using System;
using System.Collections.Generic;

namespace Custom.Pool
{
    public abstract class ObjectPoolBase<T>
        where T : class
    {
#if UNITY_EDITOR
        private readonly HashSet<T> collisionHashSet;
#endif

        private readonly int maxCapacity;
        protected readonly List<T> poolList;
        public IReadOnlyList<T> GetList => poolList;

        public ObjectPoolBase(int initialPoolCapacity = 10, int maxCapacity = 1000)
        {
            this.maxCapacity = maxCapacity;
            poolList = new List<T>(initialPoolCapacity);
#if UNITY_EDITOR
            collisionHashSet = new HashSet<T>(initialPoolCapacity);
#endif
        }
        public virtual T Pop()
        {
            T result;
            int lastIndex = poolList.Count - 1;
            if (lastIndex == -1)
            {
                result = Create();
            }
            else
            {
                result = poolList[lastIndex];
                poolList.RemoveAt(lastIndex);
#if UNITY_EDITOR
                collisionHashSet.Remove(result);
#endif
            }
            return result;
        }
        public virtual void Push(T instance)
        {
#if UNITY_EDITOR
            bool collision = collisionHashSet.Contains(instance);
            if (collision)
            {
                throw new ArgumentException($"Collision Detected. prefab : {instance}, {nameof(T)}_POOL");
            }

            collisionHashSet.Add(instance);
#endif
            if (poolList.Count < maxCapacity)
            {
                poolList.Add(instance);
            }
            else
                Destroy(instance);
        }
        protected abstract T Create();
        protected abstract void Destroy(T instance);
        public virtual void Clear()
        {
            poolList.Clear();
#if UNITY_EDITOR
            collisionHashSet.Clear();
#endif
        }
    }
}
