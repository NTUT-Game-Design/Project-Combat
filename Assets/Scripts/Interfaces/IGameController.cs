using UnityEngine;
using System.Collections;

public interface IGameController : ISystem
{
    /// <summary>
    /// Spawn player in player spawner position
    /// </summary>
    void PlayerSpawner();

    /// <summary>
    /// Spawn weapon in weapon spawner position
    /// </summary>
    void WeaponSpawner();

    /// <summary>
    /// Change terrain during game time or player remain
    /// </summary>
    void TerrainChanger();


}
