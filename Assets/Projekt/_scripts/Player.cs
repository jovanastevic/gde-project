using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public event Action ScoreChanged;
    public static event Action PlayerIsDead;
    public static event Action PlayerWon;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float speedBoost = 0.5f;
    [SerializeField] private PickupSensor pickupSensor;

    public float jumpForce = 2.0f;
    [SerializeField] private float fallMultiplier = 2.5f; 
    [SerializeField] private float lowJumpMultiplier = 2f;
    private Vector3 jump;
    private bool isGrounded;
    private Rigidbody rb;

    private List<Pickup> pullPickups = new List<Pickup>();

    private int score = 0;
    
    public static int FinalScore;

    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            ScoreChanged?.Invoke();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0f, 2.0f, 0f);
        pickupSensor.PickupEntered += PickupSensor_PickupEntered;
        pickupSensor.PickupExit += PickupSensor_PickupExit;
    }

    //Check if on Floor
    void OnCollisionStay()
    {
        isGrounded = true;
    }

    private void OnEnable()
    {
        KillSensor.KillAnyPlayer += HandleDeath;
        WinSensor.WinGame += HandleWin;
    }

    private void OnDisable()
    {
        KillSensor.KillAnyPlayer -= HandleDeath;
        WinSensor.WinGame -= HandleWin;
    }

    private void HandleWin()
    {
        FinalScore = score;
        PlayerWon?.Invoke();
    }
    private void HandleDeath()
    {
        FinalScore = score;
        PlayerIsDead?.Invoke();
    }
    private void PickupSensor_PickupEntered(Pickup pickup)
    {
        Score++;
        speed += speedBoost;
        pullPickups.Remove(pickup);
        // Destroy(pickup.gameObject);
        // pickup.gameObject.SetActive(false);
        pickup.PickedUp();
    }

    private void PickupSensor_PickupExit(Pickup pickup)
    {
        pullPickups.Remove(pickup);
    }
    
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        // if (v < 0) v = 0; //disable walking backwards
        
        
        Vector3 moveDir = new Vector3(h, 0, v).normalized;
        
        Vector3 newVelocity = moveDir * speed;
        newVelocity.y = rb.linearVelocity.y;
    
        rb.linearVelocity = newVelocity;
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
        
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); 
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            SceneManager.LoadScene("StartGameMenu");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("GamePlay"); //mit namen angeben, nicht mit ID
        }
        
    }

    
}