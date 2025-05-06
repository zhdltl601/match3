using UnityEngine;

namespace Custom.Pool
{
    public abstract class BasePoolObjectSO : ScriptableObject
    {
        [SerializeField] private int preCreateAmount;
        [SerializeField] protected GameObject prefab;
        [SerializeField] private int hash;
        public int GetPreCreate => preCreateAmount;
        public GameObject GetPrefab => prefab;
        public int GetHash => hash;
        protected virtual void OnValidate()
        {
            hash = prefab.GetHashCode();
        }
    }
}
