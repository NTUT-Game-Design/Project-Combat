using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.IO;
using System;

public class DataManager : MonoBehaviour, IDataSystem
{
    public static DataManager instance = null;
    public UnityEvent Event { get;}
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
        var a = new DataTest();
        var asd = JsonUtility.ToJson(a);
        //var ddd = System.Text.Encoding.UTF8.GetBytes(asd);
        //var filePath = Application.dataPath + "/Resources" + "/" + "Testfile.txt";
        //System.IO.File.WriteAllBytes(filePath, ddd);
        ////System.IO.File.Create(filePath);
        //Debug.Log(filePath);
        //System.IO.File.ReadAllBytes(filePath);
        //var load = JsonUtility.FromJson<DataTest>(asd);
        //Debug.Log(load.me);
        try
        {
            FileStream fsFile = new FileStream(Application.dataPath + "/Resources" + "/" + "Testfile.ini", FileMode.OpenOrCreate);
            StreamWriter swWriter = new StreamWriter(fsFile);
            //寫入數據
            swWriter.WriteLine("Hello Wrold.");
            swWriter.WriteLine(asd);
            swWriter.Close();
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    private void Awake()
    {
        #region Singleton Intial
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log( "DataManager" +" Intial.");
        }

        else
        {
            Destroy(this);
            Debug.Log("More than 1 "+ "DataManager" + " .");
        }
        #endregion
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveTest();
        }
    }
}
[SerializeField]
public class DataTest
{
    public int aere = 13213213;
    public string asdasf = "asdh;kjhlkjad";
    public bool quest;
    public name me;
    public struct name
    {
        public string first;
        public string last;
    }
    public DataTest()
    {
        me.first = "我";
        me.last = "是";
        quest = true;
    }
}