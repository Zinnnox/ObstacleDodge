using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float rotationX = 0f;
    [SerializeField] float rotationY = 0f;
    [SerializeField] float rotationZ = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(rotationX, rotationY, rotationZ);
    }
}
