using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
		
	}
	
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player1"))
        {
            FindObjectOfType<CoinsDisplay>().IncreaseCoins();
            Destroy(gameObject);
        }
    }
}
