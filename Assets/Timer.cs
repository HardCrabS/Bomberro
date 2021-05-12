using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
    [SerializeField] Text currentTime;
    Text timerText;

    float timer = 0;
    bool stopTimer = false;
	// Use this for initialization
	void Start () 
    {
        timerText = GetComponent<Text>();
	}
	void UpdateText()
    {
        timerText.text = string.Format("{0:0.0}", timer);
    }

    public float GetBestTime()
    {
        stopTimer = true;
        currentTime.text = "Your time is: " + string.Format("{0:0.0}", timer);
        FindObjectOfType<MobSpawner>().spawning = false;
        return timer;
    }
	// Update is called once per frame
	void Update () 
    {
        if (!stopTimer)
        {
            timer += Time.deltaTime;
            UpdateText();
        }
	}
}
