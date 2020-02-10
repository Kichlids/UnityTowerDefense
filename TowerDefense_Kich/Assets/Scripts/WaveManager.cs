using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Types")]
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    [Header("Spawn")]
    public GameObject spawn;

    [Header("Wave Info")]
    private Wave wave;
    private int waveIndex;
    public int numEnemiesAlive;
    public float rateOfSpawn;
    public bool nextWaveReady;
    public bool waveInProgress;

    private void Start()
    {
        waveIndex = 0;
        nextWaveReady = false;
        waveInProgress = false;
        numEnemiesAlive = 0;
        rateOfSpawn = 0.5f;
    }

    private void Update()
    {
        if (nextWaveReady)
        {
            nextWaveReady = false;
            waveInProgress = true;

            waveIndex++;
            ConstructWave();
            StartCoroutine(SpawnWave());
        }
        if (numEnemiesAlive <= 0)
        {
            waveInProgress = false;
        }
    }

    private void ConstructWave()
    {
        wave = new Wave();

        int numEnemy1 = waveIndex;
        int numEnemy2 = (int)Mathf.Pow(waveIndex, 2) - 4;
        numEnemy2 = numEnemy2 >= 0 ? numEnemy2 : 0;
        int numEnemy3 = (int)Mathf.Pow(waveIndex, 2) - 10;
        numEnemy3 = numEnemy3 >= 0 ? numEnemy3 : 0;

        for (int i = 0; i < numEnemy1; i++)
        {
            wave.enemies.Add(enemy1);
        }
        for (int i = 0; i < numEnemy2; i++)
        {
            wave.enemies.Add(enemy2);
        }
        for (int i = 0; i < numEnemy3; i++)
        {
            wave.enemies.Add(enemy3);
        }
    }

    IEnumerator SpawnWave()
    {
        numEnemiesAlive = wave.enemies.Count;

        for (int i = 0; i < wave.enemies.Count; i++)
        {
            Instantiate(wave.enemies[i], spawn.transform.position + new Vector3(0, 1.5f, 0), spawn.transform.rotation);
            yield return new WaitForSeconds(rateOfSpawn);
        }
    }

    public int GetWaveIndex()
    {
        return waveIndex;
    }

}
