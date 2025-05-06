using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Custom.Audio
{

    [CreateAssetMenu(fileName = "AudioSO", menuName = "SO/Audio/AudioSO")]
    public class AudioSO : BaseAudioSO
    {

        [Header("General")]
        public AudioClip clip;
        public AudioMixerGroup audioMixerGroup;
        public bool loop;
        public bool enableMaxCount = true;
        public int maxCount = 5;

        [Header("Global Values, StartValue")]
        [field: Range(0, 256)]     [field: SerializeField] public int Priority { get; private set; } = 128;
        [field: Range(0, 1)]       [field: SerializeField] public float Volume { get; private set; } = 1;
        [field: Range(-3, 3)]      [field: SerializeField] public float Pitch { get; private set; } = 1;
        [field: Range(-1, 1)]      [field: SerializeField] public float StreoPan { get; private set; } = 0;
        [field: SerializeField]                            private bool is3D = true;   // this was float but i changed it to bool. (do we need 2.5D?
        [field: Range(0, 1.1f)]    [field: SerializeField] public float ReverbZoneMix { get; private set; } = 1;
        [field: Range(0, 5)]       [field: SerializeField] public float DopplerLevel { get; private set; } = 0;

        [Header("3D Sound Settings")]
        [Range(0, 360)]     public int spread;

        [Range(0, 1000)]    public int minDistance = 1;
        [Range(1.01f, 1000)] public int maxDistance = 500;
        public AudioRolloffMode audioRolloffMode = AudioRolloffMode.Linear;
        [HideInInspector]   public AnimationCurve curve = AnimationCurve.Linear(0, 1, 1, 0);

        public float GetSpatialBlend => is3D ? 1 : 0;

        public override AudioSO GetAudio()
        {
            return this;
        }
    }
}
