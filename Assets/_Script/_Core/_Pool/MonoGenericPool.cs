using System;
using UnityEngine;

namespace Custom.Pool
{
    public static class MonoGenericPool<T>
        where T : MonoBehaviour, IPoolable
    {
        private static MonoPool<T> monoPool;
        /// <summary>
        /// This function must be called before any other methods are invoked.
        /// </summary>
        public static void Initialize(PoolMonoBehaviourSO prefabSO)
        {
            bool isMonoPoolInitialized = monoPool != null;
            if (isMonoPoolInitialized)
            {
                //Debug.LogError($"field:monoPool is already initialized. {prefabSO.name}");
                return;
            }
            
            T prefab = prefabSO.GetMono as T;
            monoPool = new MonoPool<T>(prefab);//, preCreate: prefabSO.GetPreCreate);

            Debug.Assert(prefab != null, $"failed to cast Mono to T. {prefabSO.name}");
        }
        public static T Pop()
        {
            return monoPool.Pop();
        }
        public static void Push(T instance)
        {
            if (instance == null) throw new ArgumentNullException("push instance is null");
            monoPool.Push(instance);
        }
        public static void Clear()
        {
            monoPool.Clear();
        }
    }
}
