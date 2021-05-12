using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject enemyPath;
    [SerializeField] float speed;
    [SerializeField] AudioClip deathSound;

    List<Transform> waypoints = new List<Transform>();
    int waypointIndex;

    void Start()
    {
        foreach (Transform point in enemyPath.transform)
        {
            waypoints.Add(point);
        }
        transform.position = waypoints[waypointIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            float movementThisFrame = speed * Time.deltaTime;
            Vector3 targetPos = waypoints[waypointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementThisFrame);

            if (transform.position == targetPos)
            {
                waypointIndex++;
            }
        }
        else
        {
            waypointIndex = 0;
        }
    }

    public void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player1"))
        {
            col.gameObject.GetComponent<Player>().Die();
            Die();
        }
        else if (col.GetComponent<Bomb>())
        {
            waypoints.Reverse();
        }
    }
}