using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    public static GameManager instance = null;

    public SettingSystem SettingSystem;
    public InputSystem InputSystem;

    private void Awake()
    {
        #region Singleton Intial
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager" + " Intial.");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("More than 1 " + "GameManager" + " .");
        }
        #endregion
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Instantiate(gameObject);
        //}
    }
}
