using System.Collections.Generic;
using UnityEngine;

namespace Custom.Pool
{
    public static class GameObjectPoolManager
    {
        private readonly static Dictionary<int, GameObjectPool> gameObjectPoolDictionary;
        private const int DICTIONARY_CAPACITIY = 10;//todo : get prefab count from so
        static GameObjectPoolManager()
        {
            gameObjectPoolDictionary = new Dictionary<int, GameObjectPool>(DICTIONARY_CAPACITIY);
        }
        private static GameObjectPool CreateDictionary(PoolGameObjectSO prefabSO)
        {
            GameObjectPool result = new GameObjectPool(prefabSO.GetPrefab);//, preCreate: prefabSO.GetPreCreate);
            gameObjectPoolDictionary.Add(prefabSO.GetHash, result);
            return result;
        }
        public static void Initialize(PoolGameObjectSO prefabSO)
        {
            int hash = prefabSO.GetHash;
            GameObject prefab = prefabSO.GetPrefab;

#if UNITY_EDITOR
            bool collisionCheckPoolMap = !gameObjectPoolDictionary.ContainsKey(prefabSO.GetHash);
            if (!collisionCheckPoolMap)
            {
                Debug.LogError($"Trying to add a key that has been added to the dictionary. {prefab.name}{hash}");
                return;
            }
#endif

            CreateDictionary(prefabSO);
        }
        public static GameObject Pop(PoolGameObjectSO prefabSO)
        {
            GameObject result;
            if (gameObjectPoolDictionary.TryGetValue(prefabSO.GetHash, out GameObjectPool value))
                result = value.Pop();
            else
            {
                Debug.LogWarning("runtimeInitializing! call func:Initialize before calling this");
                GameObjectPool gameObjectPool = CreateDictionary(prefabSO);
                result = gameObjectPool.Pop();
            }
            return result;
        }
        public static void Push(PoolGameObjectSO prefabSO, GameObject instance)
        {
            Debug.Assert(instance != null, "local: push instance is null");

            if (gameObjectPoolDictionary.TryGetValue(prefabSO.GetHash, out GameObjectPool value))
                value.Push(instance);
            else
            {
                Debug.LogWarning("runtimeInitializing! call func:Initialize before calling this");
                GameObjectPool gameObjectPool = CreateDictionary(prefabSO);
                gameObjectPool.Push(instance);
            }
        }
        public static void Clear(PoolGameObjectSO prefabSO)
        {
            gameObjectPoolDictionary[prefabSO.GetHash].Clear();
        }
        public static void ClearAll()
        {
            foreach (GameObjectPool item in gameObjectPoolDictionary.Values)
            {
                item.Clear();
            }
        }
    }
}
