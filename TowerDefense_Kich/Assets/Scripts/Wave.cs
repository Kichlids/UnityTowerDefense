using UnityEngine;
using System.Collections.Generic;


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
