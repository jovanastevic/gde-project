using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action ScoreChanged;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float pullForce = 10f;
    [SerializeField] private PickupSensor pickupSensor;
    [SerializeField] private PickupSensor pullSensor;

    private CharacterController characterController;
    public float jumpForce = 2.0f;
    public Vector3 jump;
    public bool isGrounded;
    Rigidbody rb;

    //private List<Pickup> pullPickups = new List<Pickup>();

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
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0f, 2.0f, 0f);
        // pickupSensor.PickupEntered += PickupSensor_PickupEntered;
        //pickupSensor.PickupExit += PickupSensor_PickupExit;
        // pullSensor.PickupEntered += PullSensor_PickupEntered;
        // pullSensor.PickupExit += PullSensor_PickupExit;
    }
    
    void OnCollisionStay(){
        isGrounded = true;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 movementVector = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            movementVector += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementVector += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            movementVector += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementVector += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        movementVector.Normalize();

        characterController.Move(movementVector.normalized * speed * Time.deltaTime);

        /*foreach (Pickup pickup in pullPickups)
        {
            Vector3 direction = transform.position - pickup.transform.position;
            direction.Normalize();
            pickup.Rigidbody.AddForce(direction * pullForce);
        }*/
    }
    private void PickupSensor_PickupEntered()//Pickup pickup war als Parameter
    {
        Score++;
        // pullPickups.Remove(pickup);
        // Destroy(pickup.gameObject);
        // pickup.gameObject.SetActive(false);
        //pickup.PickedUp();
    }

    private void PickupSensor_PickupExit()//Pickup pickup war als Parameter
    {
        //pullPickups.Remove(pickup);
    }

    private void PullSensor_PickupEntered()//Pickup pickup war als Parameter
    {
        //pullPickups.Add(pickup);
    }

    private void PullSensor_PickupExit()//Pickup pickup war als Parameter
    {
        // pullPickups.Remove(pickup);
    }
}