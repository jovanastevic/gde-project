using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupManager : MonoBehaviour
{
    [SerializeField] private Pickup pickupPrefab;
    [SerializeField] private float creationRadius;
    [SerializeField] private float creationDelayS;
    private ObjektPool<Pickup> pickupPool;

   void Awake()
    {
        pickupPool = new ObjektPool<Pickup>(transform, pickupPrefab);
    }

    void Start()
    {
        StartCoroutine(CreatePickupRoutine());
    }
    
    private IEnumerator CreatePickupRoutine()
    {
        while (true)
        {
            CreatePickup();
            yield return new WaitForSeconds(creationDelayS);
        }
    }

    private Pickup CreatePickup()
    {
        Vector2 position2D = Random.insideUnitCircle * creationRadius;
        Vector3 position = new Vector3(position2D.x, 1, position2D.y);
        Pickup pickup = pickupPool.GetPoolObjekt();
        pickup.transform.position = position;
        return pickup;
    }
}
