using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] public float speed = 5f;
    private Rigidbody rb;
    private Animator _animator;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && !isDead)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (Player == null) return;
        
        Vector3 direction = Player.position - transform.position;

        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
        rb.AddForce(direction.normalized * speed);
        if(rb.linearVelocity.magnitude > speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
        
        if(_animator != null)
        {
            _animator.SetFloat("speed", rb.linearVelocity.magnitude);
        }
    }
    
    void Die()
    {
        isDead = true;
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        
        if(_animator != null)
        {
            _animator.SetTrigger("die");
        }
        //
        // this.enabled = false;
        //
        // if (rb != null)
        // {
        //     rb.linearVelocity = Vector3.zero;
        //     rb.isKinematic = true;
        // }
    }
}