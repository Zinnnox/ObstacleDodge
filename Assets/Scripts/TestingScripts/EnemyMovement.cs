using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float chaseDistance = 10.0f; // How close the player needs to be
    public float moveSpeed = 5.0f; // How fast the enemy moves
    public float attackCooldown = 2.0f; // Cooldown time between attacks
    private Transform player;
    private float nextAttackTime = 0f; // Time when the enemy can attack again

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to player: " + distanceToPlayer);

        if (distanceToPlayer <= chaseDistance)
        {
            MoveTowardsPlayer();
            if (distanceToPlayer <= 1f && Time.time >= nextAttackTime)
            {
                Debug.Log("Attack condition met");
                // Attack the player
                PlayerStatsImproved playerStats = player.GetComponent<PlayerStatsImproved>();
                if (playerStats != null)
                {
                    Debug.Log("Attacking player");
                    playerStats.TakeDamage(10f);
                    Debug.Log("Player health after attack: " + playerStats.health);
                    nextAttackTime = Time.time + attackCooldown; // Set the next attack time
                }
                else
                {
                    Debug.Log("PlayerStatsImproved component not found on player");
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}