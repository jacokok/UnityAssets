using UnityEngine;

namespace Kok
{
    public abstract class AudioEvent : ScriptableObject
    {
        public abstract void Play(AudioSource source);
    }
}