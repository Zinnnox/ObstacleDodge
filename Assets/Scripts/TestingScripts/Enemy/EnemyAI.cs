using UnityEngine;
using UnityEngine.AI;            // This allows us to use Unity’s pathfinding system (NavMeshAgent).
using UnityEngine.SceneManagement; // This is for loading scenes (like a game-over screen).
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    // -- Public Variables (Visible in the Inspector) --
    public NavMeshAgent navAgent;           // The component that moves our enemy using Unity's pathfinding.

    public GameObject waypointsGroup;       // A group containing the waypoints the enemy can move to.
    private List<Transform> waypoints;       // A list of possible waypoints the enemy can move to.
    public Animator animationController;     // Controls which animation to play (walking, sprinting, idle, etc.).

    // Movement speeds and timers
    public float walkSpeed;
    public float chaseSpeed;
    public float minIdleTime;
    public float maxIdleTime;
    public float idleTime;                  // How long the enemy will stand idle.
    public float sightDistance;             // How far the enemy can "see" the player.
    public float catchDistance;             // How close the enemy needs to be to catch the player.
    public float chaseTime;
    public float minChaseTime;
    public float maxChaseTime;
    public float jumpscareTime;             // How long the jumpscare animation plays before switching scenes.

    // Booleans to keep track of our enemy's state
    public bool isWalking;                  // True if the enemy is walking to a waypoint.
    public bool isChasing;                  // True if the enemy is actively chasing the player.

    // Other references
    public Transform player;                // Reference to the player's position.
    public Vector3 raycastOffset;           // Offset for the raycast start position to avoid self-collision.
    public string deathScene;               // The scene to load if the enemy catches the player.

    // Private variables
    private Transform currentWaypoint;      // The waypoint the enemy is currently walking to.
    private Vector3 destination;            // The actual position passed to the NavMeshAgent.
    private int randomIndex;                // A random number to pick a waypoint from the list.

    void Start()
    {
        // At the start, we set the enemy to be walking and choose a random waypoint.
        InitializeWaypoints();
        isWalking = true;
        randomIndex = Random.Range(0, waypoints.Count);
        currentWaypoint = waypoints[randomIndex];
    }

    void Update()
    {
        // 1. Detect player with a Raycast
        // We shoot a ray from the enemy's position (plus an offset) towards the player to see if they are in sight.
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position + raycastOffset, directionToPlayer, out hitInfo, sightDistance))
        {
            // If the ray hits the player, start chasing!
            if (hitInfo.collider.gameObject.CompareTag("Player"))
            {
                isWalking = false;

                // Make sure we stop any ongoing Coroutines, so we don't conflict with the new chase.
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");

                StartCoroutine("chaseRoutine");
                isChasing = true;
            }
        }

        // 2. Chasing Logic
        if (isChasing)
        {
            // Move towards the player's position at chase speed.
            destination = player.position;
            navAgent.destination = destination;
            navAgent.speed = chaseSpeed;

            // Update animations to walk (since sprint is not available)
            animationController.ResetTrigger("walk");
            animationController.ResetTrigger("idle");
            animationController.SetTrigger("walk");

            // Check if we are close enough to catch the player.
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceToPlayer <= catchDistance)
            {
                // Player is caught! Hide the player and start the jumpscare.
                player.gameObject.SetActive(false);

                // Set the jumpscare animation.
                animationController.ResetTrigger("walk");
                animationController.ResetTrigger("idle");
                animationController.SetTrigger("jumpscare");

                StartCoroutine(DeathRoutine());
                isChasing = false;
            }
        }

        // 3. Walking Logic
        if (isWalking)
        {
            // The enemy heads towards its current waypoint at a walking pace.
            destination = currentWaypoint.position;
            navAgent.destination = destination;
            navAgent.speed = walkSpeed;

            // Update animations to walk
            animationController.ResetTrigger("idle");
            animationController.SetTrigger("walk");

            // If the enemy reaches the waypoint, it goes idle for a moment.
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                // Switch to idle animation and start idle timer.
                animationController.ResetTrigger("walk");
                animationController.SetTrigger("idle");

                navAgent.speed = 0; // Enemy stops moving.

                // Make sure we stop and restart the idle routine properly.
                StopCoroutine("stayIdle");
                StartCoroutine("stayIdle");

                isWalking = false;
            }
        }
    }

    private void InitializeWaypoints()
    {
        waypoints = new List<Transform>();

        // Populate the waypoints list with the positions of each child of waypointsGroup
        foreach (Transform child in waypointsGroup.transform)
        {
            waypoints.Add(child.transform);
        }
    }


    IEnumerator stayIdle()
    {
        // Wait for a random amount of time between minIdleTime and maxIdleTime.
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);

        // After waiting, the enemy picks another waypoint and starts walking again.
        isWalking = true;
        randomIndex = Random.Range(0, waypoints.Count);
        currentWaypoint = waypoints[randomIndex];
    }

    IEnumerator chaseRoutine()
    {
        // The enemy will chase for a random amount of time between minChaseTime and maxChaseTime.
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);

        // Once done chasing, the enemy goes back to walking.
        isWalking = true;
        isChasing = false;

        // Pick a new waypoint randomly.
        randomIndex = Random.Range(0, waypoints.Count);
        currentWaypoint = waypoints[randomIndex];
    }

    IEnumerator DeathRoutine()
    {
        // Wait for the jumpscare animation to finish, then load the deathScene.
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }
}
