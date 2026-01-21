using System;
using UnityEngine;

public class FrogDies : MonoBehaviour
{
    public static event Action Frogdead;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Frogdead?.Invoke();
        }
    }
}