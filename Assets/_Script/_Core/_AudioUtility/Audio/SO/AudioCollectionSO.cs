using System.Collections.Generic;
using UnityEngine;

namespace Custom.Audio
{
    [CreateAssetMenu(fileName = "AudioCollection", menuName = "SO/Audio/AudioCollection")]
    public class AudioCollectionSO : BaseAudioSO
    {
        [SerializeField] private AudioSO[] audioList;
        public AudioSO GetRandomAudio => audioList[Random.Range(0, audioList.Length)];
        public override AudioSO GetAudio()
        {
            return GetRandomAudio;
        }
    }
}
