using System;
using UnityEngine;

public class KillSensor : MonoBehaviour
{
    public static event Action KillAnyPlayer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillAnyPlayer?.Invoke();
        }
    }
}