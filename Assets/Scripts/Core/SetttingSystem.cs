using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SettingSystem : MonoBehaviour, ISettingSystem
{
    public SettingData settingData;
    public UnityEvent Event { get; }

    public void AddListener()
    {

    }
}
public class SettingData : ISettingData
{
    public void SetValue()
    {

    }
    public void GetValue()
    {

    }
}