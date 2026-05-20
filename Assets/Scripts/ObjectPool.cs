using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Object to Pool")]
    [SerializeField] GameObject BrokenVaseV1;
    [SerializeField] GameObject BrokenVaseV2;

    List<GameObject> pool1 = new List<GameObject>();
    List<GameObject> pool2 = new List<GameObject>();


    public GameObject GetVase1(Vector3 location)
    {
        return SpawnObjectFromPool(BrokenVaseV1, pool1, location);
    }
    public GameObject GetVase2(Vector3 location)
    {
        return SpawnObjectFromPool(BrokenVaseV2, pool2, location);
    }

    private GameObject SpawnObjectFromPool(GameObject objPrefab, List<GameObject> pool, Vector3 location)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                pool[i].transform.position = location;
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        GameObject obj = Instantiate(objPrefab, location, Quaternion.identity);
        pool.Add(obj);
        return obj;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
