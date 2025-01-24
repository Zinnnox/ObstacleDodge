using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraCameraScript : MonoBehaviour
{
    // Reference to the camera that shows what the player sees
    public Camera playerCamera;
    
    // How fast the camera moves when the mouse is moved
    public float sensitivity = 100f;
    
    // We'll use this variable to keep track of how much the camera has rotated up/down
    private float cameraRotationX = 0f;

    void Update()
    {
        // 1) Get the mouse movement along X (left/right) and Y (up/down) each frame.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2) Adjust the up/down rotation of the camera based on mouseY.
        //    We use "-=" so moving the mouse up rotates the camera down and vice versa.
        cameraRotationX -= mouseY * sensitivity;

        // 3) Clamp the camera’s up/down rotation so it never goes beyond -90° (looking straight up)
        //    or +90° (looking straight down).
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);

        // 4) Rotate the entire player (this object) around the Y-axis with mouseX,
        //    so you can turn left and right.
        transform.Rotate(0f, mouseX * sensitivity, 0f);

        // 5) Apply the up/down rotation to the camera itself.
        //    We only rotate on the X-axis (cameraRotationX).
        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);

        // 6) Hide the mouse cursor so it isn't visible during play.
        Cursor.visible = false;
    }
}
