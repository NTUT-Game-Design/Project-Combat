using UnityEngine;
using System.Collections;

public interface IInputSystem : ISystem
{
    /// <summary>
    /// Set the input command to new input key
    /// </summary>
    void SetKey();

    /// <summary>
    /// Get input key with input command
    /// </summary>
    void GetKey();

}
