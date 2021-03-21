using UnityEngine;
using System.Collections;

public interface IGameManager
{
    /// <summary>
    /// 
    /// </summary>
    SettingSystem SettingSystem { get; }

    /// <summary>
    /// 
    /// </summary>
    InputSystem InputSystem { get; }


}