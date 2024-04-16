using System;
using System.Collections;
using Core;
using Music;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using Utilities;

public class MusicManager : MonoBehaviour {
    public static event Action<float> OnVolumeChanged;

    public static MusicManager Instance {get; private set;}
    
    [SerializeField, Range(0,1)] private float musicVolume;
    [SerializeField] private AudioSource musicEmmiter;
    [SerializeField] private MusicRefSO musicSources;
    [SerializeField] private float musicTransitionTime;

    private AudioClip _currentMusic;

    private void Awake() {
        if(Instance){
            Destroy(gameObject);
        }else{
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }


    private void Start() {
        TransitionHandler.OnSceneChanged += PlayStateMusic;
        GameManager.OnStateChanged += PlayStateMusic;
    }

    private void OnDisable() {
        GameManager.OnStateChanged -= PlayStateMusic;
        TransitionHandler.OnSceneChanged -= PlayStateMusic;
    }
    private void Update(){
        musicEmmiter.volume = musicVolume;
    }

    public float GetMusicVolume() => musicVolume;
    public void IncreaseVolume(float value){
        musicVolume += value;
        OnVolumeChanged?.Invoke(musicVolume);
    }
    public void DecreaseVolume(float value){
        musicVolume -= value;
        OnVolumeChanged?.Invoke(musicVolume);
    }
    public void ChangeMusicVolume(float value){
        musicVolume = value;
        OnVolumeChanged?.Invoke(musicVolume);
    }

    private void PlayStateMusic(GameState state){
        Debug.Log($"{state}");
        if(state == GameState.None) return;
        if(state == GameState.MainMenu){
            ChangeMusic(musicSources.MainMenuMusic);
            return;
        }
        if(state == GameState.GameOver){
            ChangeMusic(musicSources.ToBeContinuedMusic);
            return;
        }
        ChangeMusic(musicSources.GameplayMusic);
    }

    private void ChangeMusic(AudioClip music){
        if(music == _currentMusic) return;
        
        _currentMusic = music;
        musicEmmiter.clip = _currentMusic;
        musicEmmiter.Play();
        
    }

}
