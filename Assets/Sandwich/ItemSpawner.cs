using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    public GameObject SpawnMyItem()
    {
        GameObject instance = GameObject.Instantiate(itemPrefab, transform.position, transform.rotation);

        return instance;
    }
}
