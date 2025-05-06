using UnityEngine;

namespace Custom.Pool
{
    internal class GameObjectPool : UnityObjectPool<GameObject>
    {
        public GameObjectPool(GameObject prefab, int initialPoolCapacity = 10, int maxCapacity = 1000)
            : base(prefab, initialPoolCapacity, maxCapacity)
        {
            UnityObjectPool.SceneChangePoolDestroyEvent += base.Clear;
        }
        ~GameObjectPool()
        {
            base.Clear();
            UnityObjectPool.SceneChangePoolDestroyEvent -= base.Clear;
        }

        public override GameObject Pop()
        {
            GameObject result = base.Pop();
            result.hideFlags = HideFlags.None;
            result.SetActive(true);
            return result;
        }
        public override void Push(GameObject instance)
        {
            instance.hideFlags = HideFlags.HideInHierarchy;
            instance.SetActive(false);
            base.Push(instance);
        }
        protected override GameObject Create()
        {
            GameObject result = base.Create();
            result.SetActive(false);
            return result;
        }
        public override void Clear()
        {
            foreach (GameObject item in poolList)
            {
                Object.Destroy(item);
            }
            base.Clear();
        }
    }
}
