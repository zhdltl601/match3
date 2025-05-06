using UnityEngine;

namespace Custom.Pool
{
    [CreateAssetMenu(fileName = "PoolPrefabMonoBehaviourSO", menuName = "SO/Pool/PrefabMonoBehaviourSO")]
    public class PoolMonoBehaviourSO : BasePoolObjectSO
    {
        [SerializeField] private MonoBehaviour mono;
        public MonoBehaviour GetMono => mono;
        protected override void OnValidate()
        {
            base.OnValidate();
            if (mono != null)
            {
                if (mono.gameObject != prefab)
                {
                    mono = null;
                    Debug.LogError($"Mono and prefab GameObject are different. {name}");
                }
            }
        }
    }
}
