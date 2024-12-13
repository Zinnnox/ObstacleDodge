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

        float xInc = Input.GetAxis("Horizontal") * Time.deltaTime * currentSpeed;
        float yInc = 0f;
        float zInc = Input.GetAxis("Vertical") * Time.deltaTime * currentSpeed;

        transform.Translate(xInc,yInc,zInc);

        Camera.Lens.FieldOfView = currentFOV;
    }

    private void UpdateFOV(float targetFov)
    {
        currentFOV = Mathf.Lerp(currentFOV, targetFov, deltaFOV * Time.deltaTime);
    }
}
