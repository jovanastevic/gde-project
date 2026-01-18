using System;
using UnityEngine;

public class PickupSensor : MonoBehaviour
{
    public event Action<Pickup> PickupEntered;
    public event Action<Pickup> PickupExit;
    private void OnTriggerEnter(Collider other)
    {
        Pickup pickup = other.GetComponent<Pickup>();
        if (null != pickup)
        {
            PickupEntered?.Invoke(pickup);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Pickup pickup = other.GetComponent<Pickup>();
        if (null != pickup)
        {
            PickupExit?.Invoke(pickup);
        }
    }
}
