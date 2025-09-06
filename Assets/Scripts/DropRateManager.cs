using UnityEngine;
using System.Collections.Generic;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops 
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    void OnDestroy() 
    {
        if (!gameObject.scene.isLoaded) 
        {
            return;
        }

        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        Drops rarestDrop = null;
        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                if (rarestDrop == null)
                {
                    rarestDrop = rate;
                }
                else if (rarestDrop.dropRate >= rate.dropRate)
                {
                    rarestDrop = rate;
                }
            }
        }

        if (rarestDrop != null)
        {
            Drops drops = rarestDrop;
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
        }

    }

}
