using Unity.Cinemachine;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class CCBasedPlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    const float gravity = -9.81f;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private float walkFOV = 60f;
    [SerializeField] private float sprintFOV = 80f;
    [SerializeField] private float deltaFOV = 1f;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentFOV;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentFOV = walkFOV;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateFOV();
    }

    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y <= 0)
        {
            velocity.y = 0f;
        }

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = playerCamera.transform.TransformDirection(direction);
        direction.y = 0; // Keep the movement horizontal

        characterController.Move(direction * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void UpdateFOV()
    {
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? sprintFOV : walkFOV;
        currentFOV = Mathf.Lerp(currentFOV, targetFOV, deltaFOV * Time.deltaTime);
        playerCamera.Lens.FieldOfView = currentFOV;
    }
}