using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] GameObject coinsTextGO;
    [SerializeField] GameObject doorOpenedText;
    [SerializeField] EndLevelTrigger door;
    [SerializeField] int coinsToPass;
    [TextArea(2, 4)]
    [SerializeField] string[] textsToSay;
    [SerializeField] Text text;
    [SerializeField] Text coinsText;

    CoinsDisplay coinsDisplay;

    void Start()
    {
        coinsText.text = coinsToPass.ToString();
        coinsTextGO.SetActive(true);
        doorOpenedText.SetActive(false);
        dialogBox.SetActive(false);
        coinsDisplay = FindObjectOfType<CoinsDisplay>();
    }

    void DisplayRandomSentence()
    {
        int randIndex = Random.Range(0, textsToSay.Length);
        text.text = textsToSay[randIndex];
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player1"))
        {
            dialogBox.SetActive(true);
            if (text != null)
                DisplayRandomSentence();
            CheckPlayerCoins();
        }
    }

    void CheckPlayerCoins()
    {
        if (coinsDisplay != null && coinsDisplay.Coins >= coinsToPass)
        {
            coinsTextGO.SetActive(false);
            doorOpenedText.SetActive(true);
            coinsDisplay.ResetCoins();
            GetComponent<AudioSource>().Play();
            GetComponent<Animator>().SetTrigger("CollectedCoins");
            door.isUnlocked = true;
            door.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player1"))
        {
            dialogBox.SetActive(false);
        }
    }
}