using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour 
{
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject mobPrefab;

    public bool spawning = true;
	// Use this for initialization
	void Start () 
    {
        StartCoroutine(Spawner());
	}
	
    IEnumerator Spawner()
    {
        float time = 0;
        while(spawning)
        {
            time += Time.deltaTime;
            yield return new WaitForSeconds(3f);
            int randIndex = Random.Range(0, spawnPositions.Length);
            GameObject go = Instantiate(mobPrefab, spawnPositions[randIndex].position, transform.rotation);
            go.GetComponent<RandomPathEnemy>().SetRandomSpeed(0.07f);
        }
    }
}
