using UnityEngine;

public class PickUpGenarator : MonoBehaviour
{
    [SerializeField]
    private GameObject pickUp;
    [SerializeField]
    private int numOfPickUps = 4;
    [SerializeField]
    private float radiusMin = 3;
    [SerializeField]
    private float radiusMax = 9;
    void Start()
    {
        float angle = 360f / numOfPickUps;

        int i = 0;
        while(i < numOfPickUps)
        {
            //GameObject clone = Instantiate(pickUp) as GameObject; //variante 1
            GameObject clone = Instantiate(pickUp.gameObject); //variante 2
            clone.transform.Rotate(0f, angle * i, 0f, Space.World); //rotation blabla
            clone.transform.Translate(Random.Range(radiusMin,radiusMax), 1f, 0f, Space.Self);
            i++;
        }
    }
   
}