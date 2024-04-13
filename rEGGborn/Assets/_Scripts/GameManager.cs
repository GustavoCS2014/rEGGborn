using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Inputs;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance {get; private set;}

    public enum GameStates{
        MainMenu,
        Playing,
        Paused,
        GameOver,
    }

    public static event Action<int> OnMovesIncreased;
    public int MoveCount {get; private set;}

    public GameStates State {get; private set;}

    private void Start() {
        if(Instance){
            Destroy(gameObject);
        }else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    private void Update(){
        switch(State){
            case GameStates.MainMenu:
                
            break;
            case GameStates.Playing:
            
            break;
            case GameStates.Paused:
                
            break;
            case GameStates.GameOver:
            
            break;
        }
    }





    public void IncreaseMoves(){
        MoveCount ++;
        OnMovesIncreased?.Invoke(MoveCount);
    }

    public void ChangeState(GameStates state){
        State = state;
    }
}
