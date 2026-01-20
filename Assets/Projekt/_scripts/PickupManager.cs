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
        float[] possiblePos = { -5f, 0, 5f };
 
        float playerPosZ = Player.transform.position.z + 15f;
        Vector3 targetPosition = new Vector3(possiblePos[Random.Range(0,3)], Player.transform.position.y, playerPosZ);
        Pickup pickup = pickupPool.GetPoolObjekt();
        pickup.transform.position = targetPosition;
        return pickup;
    }
}