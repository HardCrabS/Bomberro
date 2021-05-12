using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour 
{
    Player player;

    private void CheckTag()
    {
        switch (gameObject.tag)
        {
            case "Exsplosion Distance":
                {
                    player.IncreaseDistance(1f);
                }
                break;
            case "Add Bomb":
                {
                    player.IncreaseMaxBombs();
                }
                break;
            case "Shoot":
                {
                    player.StartCoroutine(player.StartShooting());
                }
                break;
            case "Shield":
                {
                    player.StartCoroutine(player.ActivateShield());
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<Player>())
        {
            player = col.GetComponent<Player>();
            CheckTag();
            Destroy(gameObject);
        }
    }
}
