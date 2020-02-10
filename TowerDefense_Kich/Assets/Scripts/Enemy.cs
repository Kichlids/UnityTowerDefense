using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    private Player player;
    private WaveManager waveManager;
    private NavMeshAgent agent;
    private GameObject spawn;
    private GameObject destination;

    public int maxHealth = 10;
    private int currentHealth;
    public float speed = 1f;

    public int gold = 1;

    private float agentStoppingDistance = 2f;

    private bool isDead;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Player>();
        waveManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WaveManager>();
        spawn = GameObject.FindGameObjectWithTag("Spawn");
        destination = GameObject.FindGameObjectWithTag("Destination");

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = agentStoppingDistance;
        agent.SetDestination(destination.transform.position);

        currentHealth = maxHealth;
        
        isDead = false;
    }

    private void Update()
    {
        if (isDead)
        {
            Destroy(gameObject);
            player.LootEnemy(gold);
            return;
        }
        if (HasReachedDestination())
        {
            Destroy(gameObject);
            player.TakeDamage(currentHealth);
            return;
        }
    }

    private bool HasReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void OnDestroy()
    {
        waveManager.numEnemiesAlive--;
    }
}
