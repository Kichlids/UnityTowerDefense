using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public Player player;
    public WaveManager waveManager;
    public BuildManager buildManager;
    public NavMeshAgent agent;
    public GameObject spawn;
    public GameObject destination;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Player>();
        waveManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WaveManager>();
        spawn = GameObject.FindGameObjectWithTag("Spawn");
        destination = GameObject.FindGameObjectWithTag("Destination");

        agent = GetComponent<NavMeshAgent>();
    }

    public bool HasReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.velocity.sqrMagnitude == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void OnDestroy()
    {
        waveManager.numEnemiesAlive--;
    }
}
