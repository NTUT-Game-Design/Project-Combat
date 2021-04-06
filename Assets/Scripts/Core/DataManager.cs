using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;
using TMPro;
public class DataManager : MonoBehaviour, IDataSystem
{
    public static DataManager instance = null;
    public UnityEvent Event { get;}
    public TextMeshProUGUI test;

    FileStream path;
    enum DataParts
    {
        Setting,
        KeyboardKeys,
        ControllerKeys
    }

    public void AddListener()
    {
       
    }
    public void SaveData()
    {

    }
    public void LoadData()
    {

    }
    public void SaveTest()
    {
        var data = ReadData(DataParts.KeyboardKeys);
        foreach(var item in data)
        {
            Debug.Log(item);
        }
    }
    private void Awake()
    {
        #region Singleton Intial
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("DataManager" +" Intial.");
        }

        else
        {
            Destroy(this);
            Debug.Log("More than 1 "+ "DataManager" + " .");
        }
        #endregion
    }
    private void Start()
    {
        try
        {
            path = new FileStream(Application.dataPath + "/Resources/Data/Data.ini", FileMode.OpenOrCreate);
        }
        catch(Exception e)
        {
            throw e;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveTest();
        }
    }
    List<string> ReadData(DataParts part)
    {
        List<string> tempString = new List<string>();
        using(StreamReader stReader = new StreamReader(path))
        {
            string str;
            while ((str = stReader.ReadLine()) != null)
            {
                tempString.Add(str);
            }
            stReader.Close();
        }
        int index = 1 + tempString.FindIndex(x => x.Contains("[" +part.ToString() + "]"));
        List<string> data = new List<string>();
        while (tempString[index] != "")
        {
            char[] tempData = tempString[index].ToCharArray();
            int start = Array.IndexOf(tempData, '=');
            int end = Array.IndexOf(tempData, ';');
            StringBuilder value = new StringBuilder();
            for(int i = start+1;  i < end; i++)
            {
                value.Append(tempData[i]);
            }
            data.Add(value.ToString());
            index++;
        }
        return data;
    }
}