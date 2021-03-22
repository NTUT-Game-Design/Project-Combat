using UnityEngine;
using System.Collections;

public interface ISceneManager : ISystem
{
    /// <summary>
    /// Change scenes
    /// </summary>
    void ChangeScene();

    /// <summary>
    /// Creat a new game with settings    
    /// </summary>
    void CreatGame();
}
