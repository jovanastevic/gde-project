using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    
    [SerializeField]
    private GameObject Player;
    private Vector3 Offset;
	
    void Start(){
        Offset = transform.position;
    }
	
    void LateUpdate()
    {
        //camera	Position vom Player + Offset
        transform.position = Player.transform.position + Offset;
    }
}
