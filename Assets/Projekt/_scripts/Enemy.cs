using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Player == null) return;

       
        Vector3 direction = Player.position - transform.position;   
        
        rb.AddForce(direction.normalized * speed);
        if(rb.linearVelocity.magnitude > speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }
}
