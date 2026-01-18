using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(15.0f,30.0f,45.0f)*Time.deltaTime);
    }
}
