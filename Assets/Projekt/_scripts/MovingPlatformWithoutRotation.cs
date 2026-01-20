using UnityEngine;

public class MovingPlatformWithoutRotation : MonoBehaviour
{
    [SerializeField] private Transform platformTransform;
    [SerializeField] private float movementSpeedX;
    [SerializeField] private float movementSpeedZ;
    
    private void FixedUpdate()
    {
        if (platformTransform.position.x is <= -8 or >= 8)
        {
            movementSpeedX *= -1;
            movementSpeedZ *= -1;
        }
        
        platformTransform.position = new Vector3(platformTransform.position.x - movementSpeedX,
            platformTransform.transform.position.y, platformTransform.position.z - movementSpeedZ);
    }
}
