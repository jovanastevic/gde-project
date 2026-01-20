using System;
using UnityEngine;


public class WinSensor : MonoBehaviour
{
    public static event Action WinGame;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WinGame?.Invoke();
        }
    }
}
