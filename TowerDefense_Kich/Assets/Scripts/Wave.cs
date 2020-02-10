using UnityEngine;
using System.Collections.Generic;

// Class that holds information about a wave

public class Wave
{
    public List<GameObject> enemies;

    public Wave()
    {
        enemies = new List<GameObject>();
        ClearWave();
    }

    public void ClearWave()
    {
        enemies.Clear();
    }
}
