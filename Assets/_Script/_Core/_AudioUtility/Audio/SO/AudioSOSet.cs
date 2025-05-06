using System;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Audio
{
    [CreateAssetMenu(fileName = "AudioSOSet", menuName = "SO/AudioSOSet")]
    public class AudioSOSet : BaseAudioSO
    {
        [SerializeField] private AudioCollectionSO[] audioCollections;
        public override AudioSO GetAudio()
        {
            throw new NotSupportedException();
        }
    }
}
