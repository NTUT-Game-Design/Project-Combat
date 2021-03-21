using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    public SettingSystem SettingSystem { get; }
    public InputSystem InputSystem { get; }
}
