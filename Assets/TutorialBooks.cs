using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBooks : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject tutorialUI;
	
    void OnTriggerEnter2D()
    {
        tutorialUI.SetActive(true);
    }

    void OnTriggerExit2D()
    {
        tutorialUI.SetActive(false);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
