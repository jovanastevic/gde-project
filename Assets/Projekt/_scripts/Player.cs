using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public event Action ScoreChanged;
    [SerializeField] private float speed = 15f;
    [SerializeField] private PickupSensor pickupSensor;

    public float jumpForce = 2.0f;
    private Vector3 jump;
    private bool isGrounded;
    private Rigidbody rb;

    private List<Pickup> pullPickups = new List<Pickup>();

    private int score = 0;

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
    }

    private void OnDisable()
    {
        KillSensor.KillAnyPlayer -= HandleDeath;
    }
    private void HandleDeath()
    {
        Debug.Log("Player is dead");
    }
    private void PickupSensor_PickupEntered(Pickup pickup)
    {
        Score++;
        pullPickups.Remove(pickup);
        // Destroy(pickup.gameObject);
        // pickup.gameObject.SetActive(false);
        pickup.PickedUp();
    }

    private void PickupSensor_PickupExit(Pickup pickup)
    {
        pullPickups.Remove(pickup);
    }
    

    //Movement Jump W A S D
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 tempVect = new Vector3(h, 0, v);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
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