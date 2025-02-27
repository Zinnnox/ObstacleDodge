using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsImproved : MonoBehaviour
{
    // STEP 1: Declare and organize variables for player stats
    [Header("Stats")] // These are the player's health, stamina, and hunger levels
    public float health = 100f;  // Player's current health
    public float stamina = 100f; // Player's stamina (used for sprinting)
    public float hunger = 100f;  // Player's hunger level (decreases over time)

    [Header("UI Sliders")] // STEP 2: Link UI sliders to display player stats
    public Slider healthBar;  // Health slider
    public Slider staminaBar; // Stamina slider
    public Slider hungerBar;  // Hunger slider

    [Header("Settings")] // STEP 3: Adjust game settings like costs, drain, and rates
    public float sprintCost = 10f;       // How much stamina sprinting costs per second
    public float hungerDrain = 5f;       // How quickly hunger decreases over time
    public float hungerDamage = 10f;     // Damage taken per second if hunger reaches 0
    public float staminaRegenRate = 15f; // How quickly stamina regenerates
    public float staminaRegenCooldown = 2f; // Cooldown before stamina starts regenerating

    private float nextStaminaRegenTime = 0f; // Timer to track when stamina can regenerate

    private bool isHealthFlashing; // Flag to prevent multiple health flashes
    public Image healthBarBackground; // For flashing health warnings

    void Start()
    {
        // STEP 4: Initialize the UI to match the starting stats
        UpdateUI();
    }

    void Update()
    {
        // STEP 5: Call functions to handle sprinting and hunger each frame
        HandleSprinting();
        HandleHunger();
        UpdateUI(); // Update the UI sliders to show new stat values
        //please pay attention
    }

    // STEP 6: Handle sprinting and stamina reduction
    void HandleSprinting()
    {
        // Check if the player is holding the Shift key to sprint
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            // Reduce stamina over time based on sprint cost
            stamina -= sprintCost * Time.deltaTime;

            // Set a cooldown timer so stamina doesn't regenerate immediately
            nextStaminaRegenTime = Time.time + 2f;
        }
        else
        {
            // Regenerate stamina if enough time has passed since sprinting
            if (Time.time >= nextStaminaRegenTime)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                // Make sure stamina doesn't go over 100
                stamina = Mathf.Clamp(stamina, 0, 100f);
            }
        }
    }

    // STEP 7: Handle hunger drain and its effects on health
    void HandleHunger()
    {
        // Gradually reduce hunger over time
        hunger -= hungerDrain * Time.deltaTime;

        // If hunger reaches 0, reduce health
        if (hunger <= 0)
        {
            health -= hungerDamage * Time.deltaTime;
        }

        // Ensure hunger and health stay between 0 and 100
        hunger = Mathf.Clamp(hunger, 0, 100f);
        health = Mathf.Clamp(health, 0, 100f);

        // If health reaches 0, call the Die function
        if (health <= 0)
        {
            Die();
        }
    }

    // STEP 8: Handle player death (what happens when health reaches 0)
    void Die()
    {
        Debug.Log("Game Over! Restarting..."); // Log a message in the console
        GameManager.Instance.ResetScene(); // Restart the scene (needs a GameManager script)
    }

    // STEP 9: Function to reduce health when the player takes damage
    public void TakeDamage(float damage)
    {
        // Reduce health by the amount of damage
        health -= damage;
        health = Mathf.Clamp(health, 0, 100f); // Keep health within the valid range

        // If health reaches 0, call the Die function
        if (health <= 0)
        {
            Die();
        }

        // Update the UI to reflect the new health value
        UpdateUI();
    }

    // STEP 10: Update the UI sliders to show the current stats
    void UpdateUI()
    {
        // Set the slider values based on the player's stats
        healthBar.value = health;
        staminaBar.value = stamina;
        hungerBar.value = hunger;
    }
 
    // New Feature 1: Flash health bar when taking damage or hunger is critical
    void FlashHealthBar()
    {
        if (!isHealthFlashing)
        {
            StartCoroutine(HealthFlashEffect());
        }
    }

    System.Collections.IEnumerator HealthFlashEffect()
    {
        isHealthFlashing = true;
        Color originalColor = healthBarBackground.color;

        // Flash red briefly
        healthBarBackground.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        healthBarBackground.color = originalColor;

        isHealthFlashing = false;
    }

    void HandleHealthWarning()
    {
        if (hunger < 10f || health < 20f)
        {
            healthBarBackground.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
        }
    }
}
