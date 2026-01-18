using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PickedUp()
    {
        gameObject.SetActive(false);
    }
    
    public Rigidbody Rigidbody{ get { return rb; } }
    
}
