using UnityEngine;

// Enumerator that stores targeting priority
public enum Targeting { first, last, closest, farthest, strongest, weakest }

/*
 *  Tower class handles targeting all eligible enemies, and attacking them according to its priority
 *  
 *  A tower updates its list of enemies, and can have different ways of attacking
 */
public class Tower : MonoBehaviour
{
    [Header("Tower attributes")]

    // Current target priority
    public Targeting targeting;

    // Maximum distance between valid enemy and the tower
    [SerializeField]
    private float range;

    // Damage dealt per hit
    [SerializeField]
    private int damage;

    // Frequency of fires
    [SerializeField]
    private float firesPerSecond;

    // Countdown for next fire
    private float fireCooldown = 0;

    // Time in seconds at which UpdateTarget function begins
    private const float TARGET_REFRESH_BEGIN = 0f;
    // Time in seconds at which UpdateTarget function refreshes
    private const float TARGET_REFRESH_RATE = 0.5f;

    // Barrel's rotation speed
    private const float ROTATION_SPEED = 10f;

    [Header("Important GameObjects")]

    // Rotating this gameObject also rotates the projectile spawnpoint
    public GameObject barrel;
    // Where projectile spawns
    public GameObject projectileSpawn;
    // Projectile GameObject to be spawned
    public GameObject projectilePrefab;

    // The current enemy being targeted
    private GameObject target;


    private void Start()
    {
        // Set all default values
        targeting = Targeting.first;
        damage = 1;
        range = 10f;
        firesPerSecond = 1;
        
        // Update target at specified rate
        InvokeRepeating("UpdateTarget", TARGET_REFRESH_BEGIN, TARGET_REFRESH_RATE);
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

    /*
     *  Rotate the transform component of the barrel gameObject to face the target enemy
     */
    private void LookAtTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(barrel.transform.rotation, lookRotation, Time.deltaTime * ROTATION_SPEED).eulerAngles;

        barrel.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    /*
     *  Find all gameObjects that are labeled "enemy" then determine
     *      the target
     */
    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        target = GetTarget(enemies, targeting);
    }

    private GameObject GetTarget(GameObject[] allEnemies, Targeting _targeting)
    {
        GameObject enemyReturn = null;
        GameObject optimalEnemy = null;

        if (_targeting == Targeting.closest)
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
                enemyReturn = optimalEnemy;
            else
                enemyReturn = null;
        }
        else if (_targeting == Targeting.farthest)
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
                enemyReturn = optimalEnemy;
            else
                enemyReturn = null;
        }
        else if (_targeting == Targeting.strongest)
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
                enemyReturn = optimalEnemy;
            else
                enemyReturn = null;
        }
        else if (_targeting == Targeting.weakest)
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
                enemyReturn = optimalEnemy;
            else
                enemyReturn = null;
        }
        else if (_targeting == Targeting.first)
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
                enemyReturn = optimalEnemy;
            else
                enemyReturn = null;
        }
        else if (_targeting == Targeting.last)
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
                enemyReturn = optimalEnemy;
            else
                enemyReturn = null;
        }

        return enemyReturn;
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.transform.position, projectileSpawn.transform.rotation);
        projectile.GetComponent<Projectile>().Initialize(GetComponent<Building>(), damage, target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
