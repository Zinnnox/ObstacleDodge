using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PrintInstruction();
    }

    // Update is called once per frame  
    void Update()
    {
        MovePlayer();
    }

    void PrintInstruction()
    {
        Debug.Log("Welcome to the game!");
        Debug.Log("Use WASD / Arrow Keys to move!");
        Debug.Log("Avoid the obstacles!");
    }

    void MovePlayer()
    {
        float xInc = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float yInc = 0f;
        float zInc = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Translate(xInc,yInc,zInc);
    }

}
