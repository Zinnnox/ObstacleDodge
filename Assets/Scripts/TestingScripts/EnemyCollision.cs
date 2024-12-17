using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public float damageAmount = 10f; // Amount of damage to deal to the player

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatsImproved playerStats = collision.gameObject.GetComponent<PlayerStatsImproved>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
            }
        }
    }
}