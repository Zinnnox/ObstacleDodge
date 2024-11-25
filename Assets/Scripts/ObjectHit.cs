using UnityEngine;

public class ObjectHit : MonoBehaviour 
{
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<MeshRenderer>().material.color = Color.red;    
            gameObject.tag = "Hit";
        }
        //Debug.Log("Something bumped into me! Agh!"); -- TESTING PURPOSE ONLY
    }
}