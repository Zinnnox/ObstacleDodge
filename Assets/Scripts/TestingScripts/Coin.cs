// File: Coin.cs
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Points awarded for this coin

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Coin collected!");
            GameManager.Instance.AddScore(coinValue); // Update the score
            Destroy(gameObject); // Remove the coin from the scene
        }
    }
}
