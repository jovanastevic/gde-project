using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupManager : MonoBehaviour
{
    [SerializeField] private Pickup pickupPrefab;
    [SerializeField] private GameObject Player;
    [SerializeField] private float creationDelayS;
    private ObjektPool<Pickup> pickupPool;

    private Vector3 player;

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
        float[] possiblePos =
            { -5f, 0, 5f };
        Vector3 playerPos = new Vector3(possiblePos[Random.Range(0, 3)], 1,
            Player.transform.position.z);
        Vector3 spawnOffset = Player.transform.forward * 10f;
        Vector3 targetPosition = playerPos + spawnOffset;
        Pickup pickup = pickupPool.GetPoolObjekt();
        pickup.transform.position = targetPosition;
        return pickup;
    }
}