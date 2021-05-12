using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPManager : MonoBehaviour 
{
    [SerializeField] GameObject winPanel;
    [SerializeField] Text winText;

    Player[] allPlayers;
    int playersLeft;

	void Start () 
    {
        allPlayers = FindObjectsOfType<Player>();
        playersLeft = allPlayers.Length;
	}
	
	public void DecreasePlayersCount(Player player)
    {
        playersLeft--;
        if(playersLeft <= 1)
        {
            Player lastPlayer = null;
            for (int i = 0; i < allPlayers.Length; i++)
            {
                print(allPlayers[i].gameObject.name);
                if (allPlayers[i] != player)
                {
                    lastPlayer = allPlayers[i];
                    break;
                }
            }
            winPanel.SetActive(true);
            winText.text = lastPlayer.gameObject.name + " wins!";
        }
    }
}
