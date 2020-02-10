using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private GameManager gameManager;

    private int maxHealth;
    private int currentHealth;
    private int initialGold;
    private int currentGold;

    private int passiveIncome = 5;
    private int passiveIncomeRateInSeconds = 5;
    private float time;

    private bool isDead;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();

        maxHealth = 9999;
        currentHealth = maxHealth;
        initialGold = 99999999;
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
            gameManager.GameOver();
        }
    }

    public bool CanPurchase(int cost)
    {
        int after = currentGold - cost;

        if (after >= 0)
        {
            currentGold = after;
            return true;
        }

        return false;
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
