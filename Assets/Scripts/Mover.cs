using System;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Mover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float sprintSpeed = 2f;
    [SerializeField] private float currentSpeed;

    [Header("Camera Settings")]
    [SerializeField] GameObject cameraInstance;
    private CinemachineCamera Camera;
    private float walkFOV = 60f;
    private float sprintFOV = 80f;
    private float currentFOV;
    [SerializeField] float deltaFOV = 1f;

    [SerializeField] Slider staminaSlider;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera = cameraInstance.GetComponent<CinemachineCamera>();
        PrintInstruction();
        SetDefaultValues();
    }
    
    // Update is called once per frame  
    void Update()
    {
        MovePlayer();
    }
    private void SetDefaultValues()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSpeed = walkSpeed;
        currentFOV = walkFOV;
    }

    void PrintInstruction()
    {
        Debug.Log("Welcome to the game!");
        Debug.Log("Use WASD / Arrow Keys to move!");
        Debug.Log("Avoid the obstacles!");
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift) && staminaSlider.value > 0)
        {
            currentSpeed = sprintSpeed;
            UpdateFOV(sprintFOV);
        }
        else
        {
            currentSpeed = walkSpeed;
            UpdateFOV(walkFOV);
        }

        float horizontal = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Camera.transform.TransformDirection(direction);
        direction.y = 0;

        transform.Translate(direction, Space.World);

        Camera.Lens.FieldOfView = currentFOV;
    }

    private void UpdateFOV(float targetFov)
    {
        currentFOV = Mathf.Lerp(currentFOV, targetFov, deltaFOV * Time.deltaTime);
    }

}
