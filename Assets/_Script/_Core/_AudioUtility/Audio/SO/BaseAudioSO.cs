using System.Collections.Generic;
using UnityEngine;

namespace Custom.Audio
{
    public abstract class BaseAudioSO : ScriptableObject
    {
        public abstract AudioSO GetAudio();
    }
}
