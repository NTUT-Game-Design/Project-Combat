﻿using System.Collections;
using UnityEngine;

public interface ISettingSystem : ISystem
{
    /// <summary>
    /// Set the value size
    /// </summary>
    void SetValue();
    /// <summary>
    /// Set the value by name
    /// </summary>
    void GetValue();
}

