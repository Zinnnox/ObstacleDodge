using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    // An array of footstep sounds (place them in here from the Inspector)
    public AudioClip[] footstepClips;
    
    // Reference to the AudioSource component on your player
    public AudioSource audioSource;
    
    // How long to wait between steps
    public float footstepDelay = 0.5f;

    // A timer to keep track of time between footstep sounds
    private float footstepTimer;

    void Update()
    {
        // 1) Decrease the timer by the time that has passed since the last frame
        footstepTimer -= Time.deltaTime;

        // 2) Check if the player is moving (using WASD or arrow keys).
        //    If the Horizontal or Vertical input axes are not zero, the player is moving.
        bool isMoving = (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f);

        // 3) If the player is moving and our footstep timer is below zero, play a new footstep sound
        if (isMoving && footstepTimer <= 0f)
        {
            PlayFootstepSound();
            // Reset the timer so we wait footstepDelay seconds before the next footstep
            footstepTimer = footstepDelay;
        }
    }

    void PlayFootstepSound()
    {
        // Pick a random clip from the array of footstep sounds
        int randomIndex = Random.Range(0, footstepClips.Length);
        audioSource.clip = footstepClips[randomIndex];

        // Play that sound clip from the AudioSource component
        audioSource.Play();
    }
}
