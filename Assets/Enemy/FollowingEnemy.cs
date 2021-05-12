using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] int healthPoints = 3;
    [SerializeField] float speed;
    [SerializeField] float chaseRadius;
    [SerializeField] GameObject coin;
    float spottedChaseRadius;

    PathRequestHandler pathRequestHandler;
    Transform player;
    Animator animator;

    Vector3[] waypoints;
    int waypointIndex;

    void Start()
    {
        spottedChaseRadius = chaseRadius;
        player = FindObjectOfType<Player>().transform;
        pathRequestHandler = FindObjectOfType<PathRequestHandler>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        Move();
    }

    bool doubledChaseRadius = false;
    void Move()
    {
        if (player != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);
            if (distToPlayer < spottedChaseRadius)
            {
                if (!doubledChaseRadius)
                {
                    spottedChaseRadius *= 2.5f;
                    doubledChaseRadius = true;
                }
                if (waypoints != null && waypointIndex <= waypoints.Length - 1)
                {
                    Vector3 targetPos = new Vector3(waypoints[waypointIndex].x, waypoints[waypointIndex].y, -1);
                    if (transform.position == targetPos)
                    {
                        waypointIndex++;
                    }
                    float movementThisFrame = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, movementThisFrame);
                }
                else
                {
                    Vector3[] positions = pathRequestHandler.GetPath(transform.position, player.position);

                    waypoints = positions;
                    waypointIndex = 0;
                }
            }
            else
            {
                spottedChaseRadius = chaseRadius;
                doubledChaseRadius = false;
            }
        }
    }

    public void Die()
    {
        healthPoints--;
        animator.SetTrigger("recievedDamage");
        if (healthPoints <= 0)
        {
            Instantiate(coin, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player1"))
        {
            Destroy(gameObject);
            col.gameObject.GetComponent<Player>().Die();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
