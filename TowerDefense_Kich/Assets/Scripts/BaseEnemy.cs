using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 *  Base class for all types of enemies
 */

public class BaseEnemy : MonoBehaviour
{
    public GameObject spawn;            // GameObject enemy spawns on
    public GameObject destination;      // GameObject enemy moves to reach
    public NavMeshAgent agent;

    public void Start()
    {
        spawn = GameObject.FindGameObjectWithTag("Spawn");
        destination = GameObject.FindGameObjectWithTag("Destination");

        agent = GetComponent<NavMeshAgent>();
    }

    // Checks if enemy has reached destination
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
        WaveManager._instance.numEnemiesAlive--;
    }
}
