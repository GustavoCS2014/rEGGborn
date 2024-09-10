using System;
using Reggborn.Core;
using UnityEngine;

namespace Reggborn.Music
{
    public class MusicManager : MonoBehaviour
    {
        public static event Action<float> OnVolumeChanged;

        public const string MUSIC_LEVEL = "MusicLevelKey";

        public static MusicManager Instance { get; private set; }

        [SerializeField, Range(0, 1)] private float musicVolume;
        [SerializeField] private AudioSource musicEmmiter;
        [SerializeField] private MusicRefSO musicSources;
        [SerializeField] private float musicTransitionTime;

        private AudioClip _currentMusic;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            musicVolume = PlayerPrefs.GetFloat(MUSIC_LEVEL, 0.5f);

        }

        private void Start()
        {
            TransitionHandler.OnSceneChanged += PlayStateMusic;
            GameManager.OnStateChanged += PlayStateMusic;
        }

        private void OnDisable()
        {
            GameManager.OnStateChanged -= PlayStateMusic;
            TransitionHandler.OnSceneChanged -= PlayStateMusic;
        }
        private void Update()
        {
            musicEmmiter.volume = musicVolume;
        }

        public float GetMusicVolume() => musicVolume;
        public void IncreaseVolume(float value)
        {
            musicVolume += value;
            OnVolumeChanged?.Invoke(musicVolume);
            PlayerPrefs.SetFloat(MUSIC_LEVEL, musicVolume);
            PlayerPrefs.Save();
        }
        public void DecreaseVolume(float value)
        {
            musicVolume -= value;
            OnVolumeChanged?.Invoke(musicVolume);
            PlayerPrefs.SetFloat(MUSIC_LEVEL, musicVolume);
            PlayerPrefs.Save();
        }
        public void ChangeMusicVolume(float value)
        {
            musicVolume = value;
            OnVolumeChanged?.Invoke(musicVolume);
            PlayerPrefs.SetFloat(MUSIC_LEVEL, musicVolume);
            PlayerPrefs.Save();
        }

        private void PlayStateMusic(GameState state)
        {
            if (state == GameState.None) return;
            if (state == GameState.MainMenu)
            {
                ChangeMusic(musicSources.MainMenuMusic);
                return;
            }
            if (state == GameState.GameOver)
            {
                ChangeMusic(musicSources.ToBeContinuedMusic);
                return;
            }
            ChangeMusic(musicSources.GameplayMusic);
        }

        private void ChangeMusic(AudioClip music)
        {
            if (music == _currentMusic) return;

            _currentMusic = music;
            musicEmmiter.clip = _currentMusic;
            musicEmmiter.Play();

        }

    }
}
