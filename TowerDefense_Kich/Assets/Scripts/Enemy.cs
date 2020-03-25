using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Enemy : BaseEnemy, IDamageable
{
    public int maxHealth = 10;
    private int currentHealth;
    public float speed = 1f;

    public int gold = 1;

    private float agentStoppingDistance = 2f;

    private bool isDead;

    protected new void Start()
    {
        base.Start();

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
            Player._instance.LootEnemy(gold);
            return;
        }
        if (HasReachedDestination())
        {
            Destroy(gameObject);
            Player._instance.TakeDamage(currentHealth);
            return;
        }
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

}
