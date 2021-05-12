using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject particle;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity *= speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player1"))
        {
            var damageable = col.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Die();
            }
            GameObject part = Instantiate(particle, transform.position, transform.rotation);
            Destroy(part, 0.5f);
            Destroy(gameObject);
        }
    }
}
