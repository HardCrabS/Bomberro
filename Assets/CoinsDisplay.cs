using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsDisplay : MonoBehaviour 
{
    Text coinsText;
    int currentCoins = 0;

	void Start () 
    {
        coinsText = GetComponent<Text>();
        UpdateText();
    }

    public int Coins
    {
        get { return currentCoins; }
    }

    public void ResetCoins()
    {
        currentCoins = 0;
        UpdateText();
    }
	
    public void IncreaseCoins()
    {
        currentCoins++;
        UpdateText();
    }

	void UpdateText () 
    {
        coinsText.text = currentCoins.ToString();
	}
}
