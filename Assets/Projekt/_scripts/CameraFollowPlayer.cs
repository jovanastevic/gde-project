using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    
    void Update () {
        transform.position = player.transform.position + new Vector3(0, 10, -15);
    }
}
