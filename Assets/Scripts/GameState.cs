using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameSceneState
{
    MainMenu,
    Levels,
    LevelEnd
    // InDialogue,
    // InGame,
    // In
} 


public enum GameControlState
{
    InteractWithGame,
    InteractWithUI
}

static public class GameState
{
    // static 
    public static GameControlState gameControlState;
}
