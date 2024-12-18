using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Chasing, Returning }
    private State currentState;

    [Header("Vision Settings")]
    public float visionRange = 10f;        // How far the enemy can see
    public float visionAngle = 60f;        // The cone angle (field of view)
    public Transform eyes;                 // The point where the enemy's vision starts
    public LayerMask playerLayer;          // Layer for detecting the player
    public LayerMask obstructionLayer;     // Layer for objects that block vision

    [Header("Chase Settings")]
    public float chaseTime = 5f;           // Time to chase after losing sight
    private float chaseTimer;

    [Header("NavMesh")]
    private NavMeshAgent agent;            // The NavMeshAgent component
    private Transform player;              // Reference to the player

    private Vector3 initialPosition;       // Enemy's starting position

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        initialPosition = transform.position;
        currentState = State.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Chasing:
                HandleChasing();
                break;
            case State.Returning:
                HandleReturning();
                break;
        }
    }

    // STEP 1: Handle the enemy's idle behavior and check for spotting the player
    void HandleIdle()
    {
        agent.isStopped = true; // Stop moving while idle

        if (PlayerInSight())
        {
            currentState = State.Chasing; // Transition to chasing
            chaseTimer = chaseTime;       // Reset chase timer
        }
    }

    // STEP 2: Handle chasing behavior
    void HandleChasing()
    {
        agent.isStopped = false; 
        agent.SetDestination(player.position); // Move towards the player

        if (PlayerInSight())
        {
            chaseTimer = chaseTime; // Reset the timer if the player is still visible
        }
        else
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0f)
            {
                currentState = State.Returning; // Return to idle if timer runs out
            }
        }
    }

    // STEP 3: Handle returning to the initial position
    void HandleReturning()
    {
        agent.SetDestination(initialPosition);

        if (Vector3.Distance(transform.position, initialPosition) < 1f)
        {
            currentState = State.Idle; // Return to idle state when back at starting position
        }
    }

    // STEP 4: Check if the player is within the enemy's cone of vision
    bool PlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - eyes.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < visionAngle / 2 && Vector3.Distance(transform.position, player.position) <= visionRange)
        {
            // Check for obstacles between the enemy and the player
            if (!Physics.Raycast(eyes.position, directionToPlayer, visionRange, obstructionLayer))
            {
                return true; // Player is visible
            }
        }

        return false;
    }

    // STEP 5: Visualize the cone of vision in the Scene view
    void OnDrawGizmos()
    {
        if (eyes == null) return;

        Gizmos.color = Color.yellow;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward;

        Gizmos.DrawRay(eyes.position, rightBoundary * visionRange);
        Gizmos.DrawRay(eyes.position, leftBoundary * visionRange);
        Gizmos.DrawLine(eyes.position, eyes.position + transform.forward * visionRange);
    }
}
