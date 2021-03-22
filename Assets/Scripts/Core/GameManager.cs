using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    static GameManager instance = null;

    public static SettingSystem SettingSystem;
    public static InputSystem InputSystem;
    public static DataSystem DataSystem;

    private void Awake()
    {
        #region Singleton Intial
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager Intial.");
        }

        else
        {
            Destroy(gameObject);
            Debug.Log("More than 1 GameManager.");
        }
        #endregion
    }
}
