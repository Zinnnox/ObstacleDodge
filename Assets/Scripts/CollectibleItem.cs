using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class CollectibleItem : MonoBehaviour
{
    [Header("UI Message")]
    public GameObject messagePanel; // We'll show/hide this panel
    public TMP_Text messageText;    // The text inside the panel (using TextMeshPro)
    public string collectedMessage = "Item Collected!";

    [Header("Attach to Player Hand")]
    public Transform playerHand;    // Assign the player's hand transform here

    private bool isCollected = false;

    // This function is called when another object (like the player) enters our trigger
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered has the "Player" tag
        if (other.CompareTag("Player") && !isCollected)
        {
            // Mark as collected so we don’t collect multiple times
            isCollected = true;

            // Show UI message 
            ShowMessage();

            // Attach this collectible to the player's hand
            AttachToPlayerHand();
        }
    }

    void ShowMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);  // Make the panel visible
            if (messageText != null)
            {
                messageText.text = collectedMessage;
            }
        }
    }

    void AttachToPlayerHand()
    {
        if (playerHand != null)
        {
            // Parent this object under the player's hand
            transform.SetParent(playerHand);

            // Reset position so it sits exactly in the hand
            transform.localPosition = Vector3.zero; 
            transform.localRotation = Quaternion.identity;

            // Optional: If you want it slightly offset in the hand, you can adjust:
            // transform.localPosition = new Vector3(0.1f, 0f, 0f); 
        }
    }
}