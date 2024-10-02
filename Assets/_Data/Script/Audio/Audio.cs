using UnityEngine;

namespace MLGameKit.Audio
{
    public abstract class Audio
    {
        [Range(0.0f, 1.0f)]
        public float DefaultVolume = 1;

        [Range(0.0f, 1.0f)]
        public float Volume = 1;

        public AudioClip Clip;

        // This will return the volume of the audio affected by the default volume
        public float GetVolume()
        {
            return Volume * DefaultVolume;
        }
    }

    public enum AudioType
    {
        Sound,
        Music,
    }
}

