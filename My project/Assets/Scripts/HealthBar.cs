using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public RectTransform healthBarRectTransform;
    public float maxHealth = 200;
    private float currentHealth;
    //public float damagePerSecond = 5;
    public TextMeshProUGUI healthText;  // Add a public variable to reference the Text component

    private float timer = 0;  // Add a timer
    public float decreaseInterval = 3f;  // Set an interval for reducing blood volume

    // Adds a public event to notify other scripts that the blood bar is zero
    public System.Action OnHealthDepleted;
    public MenuList menuList;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        //TakeDamage(damagePerSecond * Time.deltaTime);

        timer += Time.deltaTime;  // The timer adds up time
        if (timer >= decreaseInterval)  // When the timer reaches the specified interval
        {
            // TakeDamage(maxHealth * 0.1f);  // Reduced total blood volume by 0.5 times
            timer = 0;  // Reset timer
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        // If the blood bar goes to zero, an event is triggered
        if (currentHealth <= 0)
        {
            OnHealthDepleted?.Invoke();
            //menuList.Restart();
        }
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBarRectTransform.sizeDelta = new Vector2(healthPercentage * 180, healthBarRectTransform.sizeDelta.y); // Setting the maxHealth

        healthText.text = $"{healthPercentage * 100:0}%";  // Update the contents of the Text component to display the health percentage

        if (currentHealth <= 0) // Moved this check here to ensure it's checked every time the health is updated
        {
            OnHealthDepleted?.Invoke();
            //menuList.Restart();
        }
    }
    
    public float GetHealth()
    {
        return currentHealth;
    }
    
    // Adds a method to set health
    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
    }
}
