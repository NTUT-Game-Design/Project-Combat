using UnityEngine;
using System.Collections;

public interface IDataSystem : ISystem
{
    void SaveData();

    void LoadData();
}
