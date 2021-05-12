using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] float speed = 5f;
    [SerializeField] AudioClip deathSound;
    Rigidbody2D myRb;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRb.velocity = new Vector2(speed, 0);
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(myRb.velocity.x) >= Mathf.Epsilon)
        {
            transform.localScale = new Vector2(-Mathf.Sign(myRb.velocity.x), 1f);
        }
    }

    public void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        speed = -speed;
        if (col.gameObject.CompareTag("Player1"))
        {
            col.gameObject.GetComponent<Player>().Die();
            Die();
        }
    }
}
