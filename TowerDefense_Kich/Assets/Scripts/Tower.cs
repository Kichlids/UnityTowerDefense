using UnityEngine;

public enum Targeting { closest, farthest, strongest, weakest, first, last}

public class Tower : MonoBehaviour
{
    public Targeting criteria;

    public float range;
    public int damage = 1;
    public float firesPerSecond = 1;
    private float fireCooldown = 0;

    private float turnSpeed = 10f;

    private GameObject target;
    public GameObject barrel;
    public GameObject projectileSpawn;
    public GameObject projectilePrefab;

    private void Start()
    {
        criteria = Targeting.closest;
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }

    private void Update()
    {
        if (target != null)
        {
            LookAtTarget();

            if (fireCooldown <= 0)
            {
                Shoot();
                fireCooldown = 1f / firesPerSecond;
            }
            fireCooldown -= Time.deltaTime;
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(barrel.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        barrel.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GetTarget(enemies, criteria);
    }

    private GameObject GetTarget(GameObject[] allEnemies, Targeting crit)
    {
        GameObject optimalEnemy = null;

        if (crit == Targeting.closest)
        {
            float optimalDistance = Mathf.Infinity;

            foreach (GameObject enemy in allEnemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < optimalDistance)
                {
                    optimalEnemy = enemy;
                    optimalDistance = distanceToEnemy;
                }
            }

            if (optimalEnemy != null && optimalDistance <= range)
                target = optimalEnemy;
            else
                target = null;
        }
        else if (crit == Targeting.farthest)
        {
            float optimalDistance = -Mathf.Infinity;

            foreach(GameObject enemy in allEnemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy > optimalDistance && distanceToEnemy < range)
                {
                    optimalEnemy = enemy;
                    optimalDistance = distanceToEnemy;
                }
            }

            if (optimalEnemy != null)
                target = optimalEnemy;
            else
                target = null;
        }
        else if (crit == Targeting.strongest)
        {
            float optimalEnemyHealth = -Mathf.Infinity;

            foreach (GameObject enemy in allEnemies)
            {
                float enemyHealth = enemy.GetComponent<Enemy>().GetCurrentHealth();
                float enemyDist = Vector3.Distance(transform.position, enemy.transform.position);

                if (enemyHealth > optimalEnemyHealth && enemyDist < range)
                {
                    optimalEnemy = enemy;
                    optimalEnemyHealth = enemyHealth;
                }
            }

            if (optimalEnemy != null)
                target = optimalEnemy;
            else
                target = null;
        }
        else if (crit == Targeting.weakest)
        {
            float optimalEnemyHealth = Mathf.Infinity;

            foreach (GameObject enemy in allEnemies)
            {
                float enemyHealth = enemy.GetComponent<Enemy>().GetCurrentHealth();
                float enemyDist = Vector3.Distance(transform.position, enemy.transform.position);

                if (enemyHealth < optimalEnemyHealth && enemyDist < range)
                {
                    optimalEnemy = enemy;
                    optimalEnemyHealth = enemyHealth;
                }
            }

            if (optimalEnemy != null)
                target = optimalEnemy;
            else
                target = null;
        }
        else if (crit == Targeting.first)
        {
            float optimalDist = Mathf.Infinity;

            foreach (GameObject enemy in allEnemies)
            {
                float remainingDist = Vector3.Distance(enemy.transform.position, GameObject.FindGameObjectWithTag("Destination").transform.position);
                float enemyDist = Vector3.Distance(transform.position, enemy.transform.position);

                if (remainingDist < optimalDist && enemyDist < range)
                {
                    optimalEnemy = enemy;
                    optimalDist = remainingDist;
                }
            }

            if (optimalEnemy != null)
                target = optimalEnemy;
            else
                target = null;
        }
        else if (crit == Targeting.last)
        {
            float optimalDist = -Mathf.Infinity;

            foreach (GameObject enemy in allEnemies)
            {
                float remainingDist = Vector3.Distance(enemy.transform.position, GameObject.FindGameObjectWithTag("Destination").transform.position);
                float enemyDist = Vector3.Distance(transform.position, enemy.transform.position);

                if (remainingDist > optimalDist && enemyDist < range)
                {
                    optimalEnemy = enemy;
                    optimalDist = remainingDist;
                }
            }

            if (optimalEnemy != null)
                target = optimalEnemy;
            else
                target = null;
        }

        return target;
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
        projectile.GetComponent<Projectile>().SetDamage(damage);
        projectile.GetComponent<Projectile>().SetTarget(target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
