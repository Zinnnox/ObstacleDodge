using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Scorer : MonoBehaviour
{
    int hits = 0;
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag != "Hit")
        {
            hits++;
            Debug.Log("You have hit this obstacle this many times: " + hits + "!");    
        }
    }
}
