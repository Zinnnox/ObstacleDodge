using UnityEngine;

namespace Custom
{
    public class GroundCheck : MonoBehaviour
    {
        // Which layers count as "ground" (configured in Inspector)
        public LayerMask groundLayer;
        
        // The size of the box to cast for checking ground
        public Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);

        // We'll store the result here so other scripts can see if we are on ground
        public bool IsOnGround { get; private set; }

        void Update()
        {
            // We'll cast a "box" below this object to see if it hits the ground layer
            Vector3 boxCenter = transform.position + (Vector3.down * 0.5f);

            // 'CheckBox' returns true if anything in groundLayer overlaps the box region
            IsOnGround = Physics.CheckBox(boxCenter, boxSize * 0.5f, Quaternion.identity, groundLayer);
        }

        // Drawing a wireframe in the Scene View so kids can visualize it
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 boxCenter = transform.position + (Vector3.down * 0.5f);
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }
    }
}
