using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xInc = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float yInc = 0f;
        float zInc = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Translate(xInc,yInc,zInc);
    }
}
