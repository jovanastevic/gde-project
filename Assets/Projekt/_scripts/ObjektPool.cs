using System.Collections.Generic;
using UnityEngine;

public class ObjektPool<T> where T : MonoBehaviour
{
    private Transform parent;
    private T prefab;
    
    private List<T> pooledObjects = new List<T>();

    public ObjektPool(Transform parent, T prefab)
    {
        this.parent = parent;
        this.prefab = prefab;
    }

    public T GetPoolObjekt()
    {
        foreach(T poolObject in pooledObjects)
        {
            if(!poolObject.gameObject.activeInHierarchy)
            {
                poolObject.gameObject.SetActive(true);
                return poolObject;
            }
        }

        T newObject = CreateInstance();
        newObject.gameObject.SetActive(true);
        return newObject;
    }
    
    private T CreateInstance()
    {
        T instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        instance.gameObject.SetActive(false);
        pooledObjects.Add(instance);
        return instance;
    }
}
