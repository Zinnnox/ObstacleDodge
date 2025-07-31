using Unity.Cinemachine;
using UnityEngine;
using Custom;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RBBasedPlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private float walkFOV = 60f;
    [SerializeField] private float sprintFOV = 80f;
    [SerializeField] private float deltaFOV = 1f;

    private Rigidbody rb;
    public GroundCheck groundCheck;
    private bool isGrounded;
    private float currentFOV;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rotation due to physics
        currentFOV = walkFOV;

        groundCheck = new GroundCheck();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateFOV();
    }

    private void HandleMovement()
    {
        // Exit early if the player is currently dashing
        if (isDashing) return;

        // Determine movement speed based on whether the sprint key is held
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // Get input for horizontal and vertical movement
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");

        // Calculate movement direction relative to the camera's orientation
        Vector3 inputDirection = new Vector3(inputHorizontal, 0, inputVertical);
        Vector3 worldDirection = playerCamera.transform.TransformDirection(inputDirection);
        worldDirection.y = 0; // Ensure movement stays horizontal

        // Set the Rigidbody's linear velocity for movement, preserving vertical velocity
        Vector3 movementVelocity = worldDirection * currentSpeed;
        rb.linearVelocity = new Vector3(movementVelocity.x, rb.linearVelocity.y, movementVelocity.z);

        // Check for dash input and initiate dash if conditions are met
        if (Input.GetKeyDown(KeyCode.Q) && (inputHorizontal != 0 || inputVertical != 0))
        {
            StartCoroutine(Dash(worldDirection));
        }
    }

    private IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.linearVelocity = direction * dashSpeed;
            yield return null;
        }

        isDashing = false;
    }

    private void HandleJump()
    {
        if (groundCheck.IsOnGround && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Apply extra gravity manually for better control
        if (!groundCheck.IsOnGround)
        {
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    private void UpdateFOV()
    {
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? sprintFOV : walkFOV;
        currentFOV = Mathf.Lerp(currentFOV, targetFOV, deltaFOV * Time.deltaTime);
        playerCamera.Lens.FieldOfView = currentFOV;
    }
}