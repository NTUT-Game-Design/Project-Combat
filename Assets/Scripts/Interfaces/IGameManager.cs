using UnityEngine;
using System.Collections;

public interface IGameManager
{
    /// <summary>
    /// 
    /// </summary>
    ISettingSystem SettingSystem { get; }

    /// <summary>
    /// 
    /// </summary>
    IInputSystem InputSystem { get; }


}