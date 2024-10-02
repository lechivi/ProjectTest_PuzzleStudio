using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLGameKit.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "MLGameBase/Config/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        public List<Sound> Sounds = new List<Sound>();
        public List<Music> Musics = new List<Music>();

        public void SetVolume(AudioType type, float value)
        {
            switch (type)
            {
                case AudioType.Sound:
                    this.SetVolume(Sounds, value);
                    break;

                case AudioType.Music:
                    this.SetVolume(Musics, value);
                    break;
            }
        }

        public void Toggle(AudioType type, bool isOn)
        {
            switch (type)
            {
                case AudioType.Sound:
                    this.Toggle(Sounds, isOn);
                    break;

                case AudioType.Music:
                    this.Toggle(Musics, isOn);
                    break;
            }
        }

        private void Toggle<T>(List<T> audios, bool isOn) where T : Audio
        {
            foreach (var audio in audios)
            {
                audio.Volume = isOn ? audio.DefaultVolume : 0;
            }
        }

        private void SetVolume<T>(List<T> audios, float value) where T : Audio
        {
            foreach (var audio in audios)
            {
                audio.Volume = value;
            }
        }
    }
}