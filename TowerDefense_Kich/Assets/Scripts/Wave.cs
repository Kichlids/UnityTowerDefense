using UnityEngine;
using System.Collections.Generic;

// Wave class holds types of enemies to spawn

public class Wave
{
    // List of enemies to be spawned
    public List<GameObject> enemies;                

    public Wave()
    {
        enemies = new List<GameObject>();
        ClearWave();
    }

    // Clear enemy list
    public void ClearWave()
    {
        enemies.Clear();
    }
}
