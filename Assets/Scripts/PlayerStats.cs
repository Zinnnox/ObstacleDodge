using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public float stamina = 100f;
    public float hunger = 100f;

    [Header("UI Sliders")]
    public Slider healthBar;
    public Slider staminaBar;
    public Slider hungerBar;

    [Header("Settings")]
    public float sprintCost = 10f; // Stamina cost per second
    public float hungerDrain = 5f; // Hunger drain per second
    public float hungerDamage = 10f; // Damage per second when starving

    public float staminaRegenRate = 15f; // Stamina regained per second when not sprinting
    public float staminaRegenCooldown = 2f; // Time in seconds before stamina can be regenerated again

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        HandleSprinting();
        HandleHunger();
        UpdateUI();
    }

    void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            stamina -= sprintCost * Time.deltaTime;
            staminaRegenCooldown  = Time.time +2f; // set cooldown timer to current time + 2 seconds
        }
        else
        {
            if (Time.time >= staminaRegenCooldown) 
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, 100f); // Limit stamina to 0-100
            }
        }
    }

    void HandleHunger()
    {
        hunger -= hungerDrain * Time.deltaTime;

        if (hunger <= 0)
        {
            health -= hungerDamage * Time.deltaTime;
        }

        hunger = Mathf.Clamp(hunger, 0, 100f);
        health = Mathf.Clamp(health, 0, 100f);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over! Restarting...");
        GameManager.Instance.ResetScene();
    }

    void UpdateUI()
    {
        healthBar.value = health;
        staminaBar.value = stamina;
        hungerBar.value = hunger;
    }

}