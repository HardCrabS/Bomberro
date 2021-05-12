using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour 
{
    [SerializeField] GameObject pausePanel;

	public void FreezeGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void UnFreeze()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
