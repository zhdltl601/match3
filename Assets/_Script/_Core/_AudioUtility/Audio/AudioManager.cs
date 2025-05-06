using System;
using UnityEngine;
using Custom.Pool;


namespace Custom.Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        protected override MonoSingletonFlags SingletonFlag => MonoSingletonFlags.DontDestroyOnLoad; 

        [SerializeField] private PoolMonoBehaviourSO poolPrefabMonoBehaviourSO;
        //private static AudioEmitter audioEmitter2D;
        protected override void Awake()
        {
            base.Awake();
            MonoGenericPool<AudioEmitter>.Initialize(poolPrefabMonoBehaviourSO);
            //audioEmitter2D = MonoGenericPool<AudioEmitter>.Pop();
            //audioEmitter2D.name = "audioEmitter2D";
        }
        public static AudioEmitter GetEmitter()
        {
            AudioEmitter audioEmitter = MonoGenericPool<AudioEmitter>.Pop();
            return audioEmitter;
        }
        public static AudioEmitter GetEmitter(BaseAudioSO baseAudio)
        {
            AudioSO audioSO = baseAudio.GetAudio();

            AudioEmitter audioEmitter = MonoGenericPool<AudioEmitter>.Pop();
            audioEmitter.Initialize(audioSO);
            return audioEmitter;
        }
        public static AudioEmitter PlayWithInit(BaseAudioSO baseAudio, bool destroyOnEnd = false)
        {
            AudioSO audioSO = baseAudio.GetAudio();

            Debug.Assert(audioSO != null, "audioSO is null");

            AudioEmitter audioEmitter = GetEmitter();
            audioEmitter.PlayWithInit(audioSO, destroyOnEnd);

            return audioEmitter;
        }
        public static AudioEmitter PlayOneShotWithInit(AudioSO audioSO)
        {
            throw new NotImplementedException("no");
        }

    }
}
