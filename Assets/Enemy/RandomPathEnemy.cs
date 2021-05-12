using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] float speed;
    [SerializeField] AudioClip deathSound;

    PathRequestHandler pathRequestHandler;
    GridA grid;

    List<Vector3> waypoints;
    int waypointIndex;
    void Start()
    {
        pathRequestHandler = FindObjectOfType<PathRequestHandler>();
        grid = FindObjectOfType<GridA>();
    }

    float slowestSpeed = 1.5f, fastestSpeed = 3.5f;
    public void SetRandomSpeed(float increment)
    {
        slowestSpeed += increment; fastestSpeed += increment;
        speed = Random.Range(slowestSpeed, fastestSpeed);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (waypoints != null && waypointIndex <= waypoints.Count - 1)
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
            Vector3[] positions;
            do
            {
                Vector3 randWalkablePos = GetRandomWalkablePos();
                positions = pathRequestHandler.GetPath(transform.position, randWalkablePos);
            }
            while (positions == null);
            waypoints = FillFromArray(positions);
            waypointIndex = 0;
        }
    }

    Vector3 GetRandomWalkablePos()
    {
        int randIndexX;
        int randIndexY;
        do
        {
            randIndexX = Random.Range(0, grid.grid.GetLength(0));
            randIndexY = Random.Range(0, grid.grid.GetLength(1));
        }
        while (!grid.grid[randIndexX, randIndexY].walkable);

        return grid.grid[randIndexX, randIndexY].worldPosition;
    }

    List<Vector3> FillFromArray(Vector3[] positions)
    {
        List<Vector3> waypoints = new List<Vector3>();
        for (int i = 0; i < positions.Length; i++)
        {
            waypoints.Add(positions[i]);
        }
        return waypoints;
    }
    public void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);

        if (GetComponent<EndlessDrop>())
            GetComponent<EndlessDrop>().SpawnBonus();
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
            waypointIndex = waypoints.Count - waypointIndex;
        }
    }
}