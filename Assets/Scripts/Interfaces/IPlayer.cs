using UnityEngine;
using System.Collections;

public interface IPlayer : ISystem
{
    /// <summary>
    /// Make player move
    /// </summary>
    void Move();

    /// <summary>
    /// Make player jump
    /// </summary>
    void Jump();

    /// <summary>
    /// Excute attack with selectarrow
    /// </summary>
    void Attack();

    /// <summary>
    /// Excute defence with selectarrow
    /// </summary>
    void Defence();

    /// <summary>
    /// Which position selected now
    /// </summary>
    void SelectArrow();

}
