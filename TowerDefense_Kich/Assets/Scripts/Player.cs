using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int maxHealth;
    public int currentHealth;
    public int initialGold;
    public int currentGold;

    public int passiveIncome = 5;
    public int passiveIncomeRateInSeconds = 5;
    private float time;

    public bool isDead;

    public static Player _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        maxHealth = 1;
        currentHealth = maxHealth;
        initialGold = 9999;
        currentGold = initialGold;

        time = 0;

        isDead = false;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time > passiveIncomeRateInSeconds)
        {
            currentGold += passiveIncome;
            time = 0;
        }

        if (isDead)
        {
            GameManager._instance.GameOver();
        }
    }

    public bool CanPurchase(int cost)
    {
        int after = currentGold - cost;

        return after >= 0;
    }

    public void Purchase(int cost)
    {
        currentGold -= cost;
    }

    public void SellBuilding(GameObject building)
    {
        int sell = building.GetComponent<Building>().sell;

        if (sell >= 0)
        {
            currentGold += sell;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void LootEnemy(int gold)
    {
        currentGold += gold;
    }

    public int GetLives()
    {
        return currentHealth;
    }

    public int GetGold()
    {
        return currentGold;
    }
}
