using UnityEngine;
using System.Linq;

namespace MLGameKit.Audio
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        public bool IsSoundOn => isSoundOn();
        public bool IsMusicOn => isMusicOn();

        public float SoundVolume => GetVolume(AudioType.Sound);
        public float MusicVolume => GetVolume(AudioType.Music);

        [SerializeField] private AudioConfig audioLibrary;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundSource;
        [SerializeField] private AudioSource loopingSource;

        [SerializeField] private float defautMusicVolume = 0.5f;

        private const string SOUND_TOGGLE_KEY = "SoundToggle";
        private const string SOUND_VOLUME_KEY = "SoundVolume";

        private const string MUSIC_TOGGLE_KEY = "MusicToggle";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";

        protected override void Awake()
        {
            if (this.audioLibrary != null)
            {
                this.audioLibrary.SetVolume(AudioType.Sound, GetVolume(AudioType.Sound));
                this.audioLibrary.SetVolume(AudioType.Music, GetVolume(AudioType.Music));
            }
        }

        private void Start()
        {
            PlayMusic(MusicType.Bgm1);
        }

        public void PlaySound(SoundType type)
        {
            Sound sound = GetSound(type);

            if (sound != null)
            {
                soundSource.PlayOneShot(sound.Clip, sound.GetVolume());
            }
        }

        public void PlayMusic(MusicType type)
        {
            Music music = this.GetMusic(type);
            if (music == null) return;
            musicSource.clip = music.Clip;
            musicSource.volume = music.Volume;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void PlayLoopingSource(MusicType type)
        {
            Music music = this.GetMusic(type);
            if (music == null) return;
            loopingSource.clip = music.Clip;
            loopingSource.volume = music.Volume;
            loopingSource.loop = true;
            loopingSource.Play();
        }

        public void PauseMusicSource()
        {
            musicSource.Pause();
        }

        public void PauseLoopingSource()
        {
            loopingSource.Pause();
        }

        public void PlayMusicSource()
        {
            musicSource.Play();
        }

        public void OnSoundVolumeChanged(float value)
        {
            audioLibrary.SetVolume(AudioType.Sound, value);
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, value);
        }

        public void OnMusicVolumeChanged(float value)
        {
            audioLibrary.SetVolume(AudioType.Music, value);
            musicSource.volume = value;
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
        }

        public bool Toggle(AudioType type)
        {
            string AUDIO = type == AudioType.Sound ? SOUND_TOGGLE_KEY : MUSIC_TOGGLE_KEY;
            AudioSource source = type == AudioType.Sound ? soundSource : musicSource;

            bool isOn = Mathf.Abs(PlayerPrefs.GetInt(AUDIO, 1) - 1) == 1;
            audioLibrary.Toggle(type, isOn);

            source.mute = !isOn;
            PlayerPrefs.SetFloat(AUDIO, isOn ? 1 : 0);

            return isOn;
        }

        public Sound GetSound(SoundType type)
        {
            return audioLibrary.Sounds.Where(s => s.Type == type).FirstOrDefault();
        }

        public Music GetMusic(MusicType type)
        {
            return audioLibrary.Musics.Where(s => s.Type == type).FirstOrDefault();
        }

        public float GetVolume(AudioType type)
        {
            string KEY = type == AudioType.Sound ? SOUND_VOLUME_KEY : MUSIC_VOLUME_KEY;

            float defaultVolume = 1;
            return PlayerPrefs.GetFloat(KEY, defaultVolume);
        }

        private bool isSoundOn()
        {
            return PlayerPrefs.GetFloat(SOUND_TOGGLE_KEY, 1) > 1;
        }

        private bool isMusicOn()
        {
            return PlayerPrefs.GetFloat(MUSIC_TOGGLE_KEY, defautMusicVolume) > 0;
        }

        private void OnDisable()
        {
            //listenerManager.Unregister(ListenType.Win, (t) => PlaySound(SoundType.LevelSuccess));
            //listenerManager.Unregister(ListenType.Lose, (t) => PlaySound(SoundType.LevelFailed));
        }
    }
}


