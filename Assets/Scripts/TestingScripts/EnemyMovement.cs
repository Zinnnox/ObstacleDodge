using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float chaseDistance = 10.0f; // How close the player needs to be
    public float moveSpeed = 5.0f; // How fast the enemy moves
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance)
        {
            MoveTowardsPlayer();
            if (distanceToPlayer <= 0.5f)
            {
                // Attack the player
                // find the player's PlayerStats script and call the TakeDamage method
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                playerStats.TakeDamage(10f);
                
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}