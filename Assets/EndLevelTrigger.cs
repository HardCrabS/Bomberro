using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour 
{
    public bool isUnlocked = false;
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player1") && isUnlocked)
        {
            GameData gameData = FindObjectOfType<GameData>();
            if (gameData != null)
                gameData.UnlockNextLevel();
            FindObjectOfType<LevelManager>().LoadNextScene();
        }
    }
}
