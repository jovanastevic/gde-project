using System;
using UnityEngine;

public class PickupSensor : MonoBehaviour
{
    public event Action<Collider> PickupEntered;
    public event Action<Collider> PickupExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            PickupEntered?.Invoke(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            PickupExited?.Invoke(other);
        }
    }
}