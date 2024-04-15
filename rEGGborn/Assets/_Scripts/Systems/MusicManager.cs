using System;
using System.Collections;
using Core;
using Music;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using Utilities;

public class MusicManager : MonoBehaviour {
    public static MusicManager Instance {get; private set;}
    
    public float MusicVolume {get; private set;}

    [SerializeField] private AudioSource musicEmmiter;
    [SerializeField] private AudioSource secondMusicEmmiter;
    [SerializeField] private MusicRefSO musicSources;
    [SerializeField] private float musicTransitionTime;

    private AudioClip _currentMusic;
    private GameManager _gameManager;

    private void Awake() {
        if(Instance){
            Destroy(gameObject);
        }else{
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start() {
        if(GameManager.Instance)
            _gameManager = GameManager.Instance;

        TransitionHandler.OnSceneChanged += OnSceneChangedEvent;
        GameManager.OnStateChanged += OnStateChangedEvent;
        // PlayStateMusic(_gameManager.State);
    }


    private void OnDisable() {
        GameManager.OnStateChanged -= OnStateChangedEvent;
        TransitionHandler.OnSceneChanged -= OnSceneChangedEvent;
    }

    private void OnSceneChangedEvent(GameState state)
    {
        PlayStateMusic(state);
    }

    public void ChangeMusicVolume(float value) => MusicVolume = value;

    private void OnStateChangedEvent(GameState state)
    {
        PlayStateMusic(state);
    }

    private void PlayStateMusic(GameState state){
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

    // private IEnumerator MusicTranstion(AudioSource from, AudioSource to){
        
    // }
}
