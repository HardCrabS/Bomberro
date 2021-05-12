using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToMainMenu : MonoBehaviour 
{
    void OnTriggerEnter2D()
    {
        FindObjectOfType<LevelManager>().MainMenu();
    }
}
