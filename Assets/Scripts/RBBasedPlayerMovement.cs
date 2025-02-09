using Unity.Cinemachine;
using UnityEngine;
using Custom;

[RequireComponent(typeof(Rigidbody))]
public class RBBasedPlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityMultiplier = 2f;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private float walkFOV = 60f;
    [SerializeField] private float sprintFOV = 80f;
    [SerializeField] private float deltaFOV = 1f;

    private Rigidbody rb;
    public GroundCheck groundCheck;
    private bool isGrounded;
    private float currentFOV;

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
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = playerCamera.transform.TransformDirection(direction);
        direction.y = 0; // Keep the movement horizontal

        Vector3 move = direction * speed;
        Vector3 velocity = new Vector3(move.x, rb.linearVelocity.y, move.z); // Preserve the y-component of the velocity
        rb.linearVelocity = velocity;
    }

    private void HandleJump()
    {
        //isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (groundCheck.IsOnGround && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Apply extra gravity manually for better control
        if (!isGrounded)
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