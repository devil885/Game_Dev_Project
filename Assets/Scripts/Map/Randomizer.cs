using UnityEngine;
using System.Collections.Generic;
//using System;

public class Randomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    void Start()
    {
        SpawnProps();
    }

    
    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach (GameObject spawn in propSpawnPoints)
        {
            int random = UnityEngine.Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[random], spawn.transform.position, Quaternion.identity);
            prop.transform.parent = spawn.transform;
        }
    }
}
