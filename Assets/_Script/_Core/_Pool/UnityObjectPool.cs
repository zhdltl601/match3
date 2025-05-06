using UnityEngine;
using UnityEngine.SceneManagement;

namespace Custom.Pool
{
    internal delegate void UnityPoolDestroyDelegate();
    internal static class UnityObjectPool
    {
        private static Transform baseParent;
        private const string BASE_PARENT_NAME = "_UnityObjectPool_BaseParent";
        internal static Transform GetBaseParent => baseParent;
        internal static event UnityPoolDestroyDelegate SceneChangePoolDestroyEvent;
        static UnityObjectPool()
        {
            CreateBaseParent();
            //Object.DontDestroyOnLoad(baseParent); // scene dependent
            SceneManager.sceneLoaded += (_, _) =>
            {
                if (baseParent == null)
                {
                    SceneChangePoolDestroyEvent?.Invoke();
                    CreateBaseParent();
                }
            };
            static void CreateBaseParent()
            {
                baseParent = new GameObject(BASE_PARENT_NAME).transform;
                baseParent.hideFlags = HideFlags.HideInHierarchy;
            }
        }
    }
    internal abstract class UnityObjectPool<T> : ObjectPoolBase<T>
        where T : Object
    {
        protected readonly T prefab;
        public UnityObjectPool(T prefab, int initialPoolCapacity = 10, int maxCapacity = 1000) : base(initialPoolCapacity, maxCapacity)
        {
            this.prefab = prefab;

            //for (int i = 0; i < preCreate; i++)
            //{
            //    Push(Create());
            //}
        }
        protected override T Create()
        {
            T result = Object.Instantiate(prefab);
            result.hideFlags = HideFlags.HideInHierarchy;
            return result;
        }
        protected override void Destroy(T instance)
        {
            Object.Destroy(instance);
        }
    }
}
