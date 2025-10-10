using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth;
    int currentHealth;

    public RectTransform healthBar;
    float barWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        barWidth = healthBar.anchorMax.x;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarManager();
    }

    public void PlayerHealth(int healthModifier)
    {
        currentHealth += healthModifier;

        if (currentHealth <= 0)
        { }
            //DeathScene();
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void HealthBarManager()
    {
        currentHealth = ((currentHealth > maxHealth) ? maxHealth : (currentHealth < 0) ? 0 : currentHealth);

        float x = ((currentHealth * (100f / maxHealth)) * (1f / barWidth)) / 100f;

        healthBar.anchorMax = new Vector2(x, healthBar.anchorMax.y);
    }
}
