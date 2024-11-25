using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] float timeToWait = 3;
    MeshRenderer myMeshRenderer;
    Rigidbody myRigidBody;

    private void Start() 
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        myRigidBody = GetComponent<Rigidbody>();

        myRigidBody.useGravity = false;
        myMeshRenderer.enabled = false;    
    }

    // Update is called once per frame
    void Update()
    {
        float timePassed = Time.time;

        //Debug.Log(timePassed);

        if (timePassed > timeToWait)
        {
            myMeshRenderer.enabled = true;
            myRigidBody.useGravity = true;
        }

    }
}
