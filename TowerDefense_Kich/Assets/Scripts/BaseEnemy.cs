using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public GameObject spawn;
    public GameObject destination;
    public NavMeshAgent agent;

    public void Start()
    {
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
        WaveManager._instance.numEnemiesAlive--;
    }
}
