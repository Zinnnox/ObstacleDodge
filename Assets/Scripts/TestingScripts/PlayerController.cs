// File: PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float fallThreshold = -10f; // Height at which the player respawns

    void Update()
    {
        // Check if the player has fallen too far
        if (transform.position.y < fallThreshold)
        {
            Debug.Log("Player has fallen. Resetting the scene.");
            GameManager.Instance.ResetScene();
        }
    }
}
