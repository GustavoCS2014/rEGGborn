using Reggborn.Objects;
using UnityEngine;
using UnityEngine.Audio;
namespace Reggborn.Core
{
    public sealed class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField, Range(0f, 1f)] private float SFXVolume;
        [SerializeField] private AudioMixerGroup SFXMixerGroup;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            //Subscribe to sounds

            MovableRock.PlayMoveSound += PlaySFX;
            Spikes.PlayUpSound += PlaySFX;
        }

        private void OnDisable()
        {
            //Unsubscribe to sounds

            MovableRock.PlayMoveSound -= PlaySFX;
            Spikes.PlayUpSound -= PlaySFX;
        }

        private void PlaySFX(AudioClip sound, Vector3 pos)
        {
            PlaySound(sound, pos, SFXVolume, SFXMixerGroup);
        }

        private void PlaySound(AudioClip sound, Vector3 pos, float volume = 1, AudioMixerGroup group = null)
        {
            PlayClipAtPoint(sound, pos, volume, group);
        }

        public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f, AudioMixerGroup group = null)
        {
            if (clip == null) return;
            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.position = position;
            AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            if (group != null)
                audioSource.outputAudioMixerGroup = group;
            audioSource.clip = clip;
            audioSource.spatialBlend = 1f;
            audioSource.volume = volume;
            audioSource.Play();
            Object.Destroy(gameObject, clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
        }

    }
}