using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessDrop : MonoBehaviour 
{
    [SerializeField] float dropChance = 50;
    [SerializeField] GameObject[] bonuses;
    // Use this for initialization
    public void SpawnBonus()
    {
        float chance = Random.Range(0, 100);
        if (chance < dropChance)
        {
            int randIndex = Random.Range(0, bonuses.Length);
            Instantiate(bonuses[randIndex], transform.position, transform.rotation);
        }
    }
}
