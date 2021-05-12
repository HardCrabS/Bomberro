using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour 
{
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.GetComponent<IDamageable>() != null)
        {
            col.GetComponent<IDamageable>().Die();
        }
    }
}
