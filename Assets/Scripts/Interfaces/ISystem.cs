using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// System Base
/// </summary>
public interface ISystem
{
    /// <summary>
    /// Add a listener to receive other event's boardcast
    /// </summary>
    void AddListener();

    /// <summary>
    /// A readonly event use to send boradcast who listen me
    /// </summary>
    UnityEvent Event { get;}
}