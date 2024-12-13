using UnityEngine;

public class firstPersonCamera : MonoBehaviour 
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    
    private float xRotation = 0f;

    private void Start() 
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Handle vertical rotation with constraints
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents "neck breaking" by limiting up/down look angle
        
        // Apply vertical rotation to camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Rotate player body horizontally
        playerBody.Rotate(Vector3.up * mouseX);
    }
}