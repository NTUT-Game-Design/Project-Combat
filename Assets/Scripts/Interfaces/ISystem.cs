using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// System Base
/// </summary>
public interface ISystem
{
    //Can reciver boardcast
    void AddListener(UnityAction action, Button button);
    void AddListener(UnityAction action, UnityEvent Event);

    //Can invoke boardcast
    UnityEvent Event { get; }
}

public interface ISettingSystem : ISystem
{
    //Settings value
    string Resolution { get; set; }//EX: "1920*1080"
    float MainVolume { get; set; }
    float BGMVolume { get; set; }
    float VFXVolume { get; set; }

    //Setting functions
    void SetValue(string value);
    void SetValue(float value);
}
public interface IInputSystem : ISystem
{

}
public interface IGameGenerator : ISystem
{
    void BuildGame(string GameMode, int PlayerNumbers, string Maps, int teams, string GameOptions);
}
public interface IGameController : ISystem
{
    void PlayerSpawner(GameObject Player, Transform postions);
    WeaponSpawner WeaponSpawner { get; }
    TerrainChanger TerrainChanger { get; }

}
public interface IPlayer : ISystem
{
    void Move();
    void Jump();
    void Attack(GameObject Weapon);
    void Defence(GameObject Enemy);
    int SelectArrow();
}



public interface IGameManager
{

}
public interface ISceneManager
{

}
public interface IAudioManager
{

}