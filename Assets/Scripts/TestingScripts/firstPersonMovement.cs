
using UnityEngine;

public class firstPersonMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera playerCamera;

    private void Update()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get camera's forward and right vectors, but ignore vertical component
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calculate movement direction relative to where we're looking
        Vector3 movement = forward * vertical + right * horizontal;
        
        // Apply movement
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
