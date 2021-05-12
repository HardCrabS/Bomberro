using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] int level;
    [SerializeField] bool isUnlocked = true;
    [SerializeField] Sprite lockedLevelSprite;
    [SerializeField] Sprite unlockedSprite;

    GameData gameData;

    void Start()
    {
        gameData = FindObjectOfType<GameData>();

        LoadData();
        SetButton();
    }

    void LoadData()
    {
        if (gameData != null)
        {
            isUnlocked = gameData.saveData.isUnlocked[level - 1];
        }
    }

    void SetButton()
    {
        if (!isUnlocked)
        {
            if (transform.GetChild(0) != null)
                transform.GetChild(0).gameObject.SetActive(false);
            Button button = GetComponent<Button>();
            GetComponent<Image>().sprite = lockedLevelSprite;
            button.interactable = false;
            var colors = button.colors;
            colors.normalColor = Color.red;
            button.colors = colors;
        }
        else
        {
            if (transform.GetChild(0) != null)
                transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<Image>().sprite = unlockedSprite;
            Button button = GetComponent<Button>();
            button.onClick.AddListener(LoadMyScene);
        }
    }

    void LoadMyScene()
    {
        gameData.currentLevel = level;
        FindObjectOfType<LevelManager>().LoadConcreteScene("Level " + (level - 1).ToString());
    }
}
