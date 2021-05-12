using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    float distance;
    [SerializeField] float bonusDropChance = 25f;
    [SerializeField] LayerMask blocksLayers;
    [SerializeField] LayerMask damageables;
    [SerializeField] Bonus[] bonusArray;
    [SerializeField] GameObject explosion;

    GameObject destoyableTilemap;
    AudioSource audioSource;

    Player player;
    Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };
    Tilemap tilemap;
    GridA grid;
    // Use this for initialization
    void Start()
    {
        transform.position = GetCenterOfTile(transform);
        destoyableTilemap = GameObject.FindWithTag("DestroyableTilemap");
        tilemap = destoyableTilemap.GetComponent<Tilemap>();
        audioSource = GetComponent<AudioSource>();

        distance = player.GetDistance();
        player.IncreaseBombs();
    }

    Vector3 GetCenterOfTile(Transform go)
    {
        float xPos = (int)go.position.x + (0.5f * Mathf.Sign(go.position.x));
        float yPos = (int)go.position.y + (0.5f * Mathf.Sign(go.position.y));
        Vector3 roundPos = new Vector3(xPos, yPos, -1);

        return roundPos;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.up * distance, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * distance, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * distance, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * distance, Color.red);
    }

    void CallInAnimationEvent()
    {
        Explode(Vector2.zero);
    }

    bool exploaded = false;
    void Explode(Vector2 directionToSkip)
    {
        exploaded = true;

        if (player != null && GetCenterOfTile(player.transform) == transform.position)
        {
            player.Die();
        }

        GameObject explosionVFX = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(explosionVFX, 0.5f);
        audioSource.Play();

        foreach (Vector2 direction in directions)
        {
            if (direction == directionToSkip)
            {
                continue;
            }
            Vector3Int tilePos = new Vector3Int();
            RaycastHit2D hitForBlocks = Physics2D.Raycast(transform.position, direction, distance, blocksLayers);
            RaycastHit2D hitForDamageables = Physics2D.Raycast(transform.position, direction, distance, damageables);

            if (hitForBlocks.collider != null)
            {
                if (hitForDamageables.collider != null)
                {
                    if (Mathf.Abs(Vector2.Distance(transform.position, hitForDamageables.point))
                        < Mathf.Abs(Vector2.Distance(transform.position, hitForBlocks.point)))
                        hitForDamageables.collider.GetComponent<IDamageable>().Die();
                }

                if (direction == Vector2.up)
                {
                    if (hitForBlocks.collider.tag == "Undestroyable")
                    {
                        StartCoroutine(ExplosionLine(Vector2.up, Mathf.Abs(hitForBlocks.point.y) - Mathf.Abs(transform.position.y)));
                        continue;
                    }
                    tilePos = new Vector3Int(Mathf.RoundToInt(hitForBlocks.point.x - 0.5f),
                                              Mathf.RoundToInt(hitForBlocks.point.y), 0);
                }
                else if (direction == Vector2.right)
                {
                    if (hitForBlocks.collider.tag == "Undestroyable")
                    {
                        StartCoroutine(ExplosionLine(Vector2.right, Mathf.Abs(hitForBlocks.point.x) - Mathf.Abs(transform.position.x)));
                        continue;
                    }
                    tilePos = new Vector3Int(Mathf.RoundToInt(hitForBlocks.point.x),
                                              Mathf.RoundToInt(hitForBlocks.point.y - 0.5f), 0);
                }
                else if (direction == Vector2.down)
                {
                    if (hitForBlocks.collider.tag == "Undestroyable")
                    {
                        StartCoroutine(ExplosionLine(Vector2.down, Mathf.Abs(transform.position.y) - Mathf.Abs(hitForBlocks.point.y)));
                        continue;
                    }
                    tilePos = new Vector3Int(Mathf.RoundToInt(hitForBlocks.point.x - 0.5f),
                                              Mathf.RoundToInt(hitForBlocks.point.y - 1f), 0);
                }
                else if (direction == Vector2.left)
                {
                    if (hitForBlocks.collider.tag == "Undestroyable")
                    {
                        StartCoroutine(ExplosionLine(Vector2.left, Mathf.Abs(transform.position.x) - Mathf.Abs(hitForBlocks.point.x)));
                        continue;
                    }
                    tilePos = new Vector3Int(Mathf.RoundToInt(hitForBlocks.point.x - 1),
                                              Mathf.RoundToInt(hitForBlocks.point.y - 0.5f), 0);
                }

                float distance = direction.x * (Mathf.Abs(transform.position.x) - Mathf.Abs(hitForBlocks.point.x) - 0.5f * Mathf.Sign(direction.x))
                    + direction.y * (Mathf.Abs(transform.position.y) - Mathf.Abs(hitForBlocks.point.y) - 0.5f * Mathf.Sign(direction.y));

                StartCoroutine(ExplosionLine(direction, Mathf.Abs(distance)));
                tilemap.SetTile(tilePos, null);

                if (grid == null)
                    grid = FindObjectOfType<GridA>();
                if (grid != null)
                    grid.SetNodeWalkable(new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f));
                if (hitForBlocks.collider.CompareTag("DestroyableTilemap"))
                {
                    SpawnBonus(tilePos);
                }
                ExplodeNeighborBomb(hitForBlocks, direction);
            }
            else
            {
                if (hitForDamageables.collider != null)
                {
                    hitForDamageables.collider.GetComponent<IDamageable>().Die();
                }
                StartCoroutine(ExplosionLine(direction, distance));
            }
        }
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        player.DecreaseBombs();
        Destroy(gameObject, 0.5f);
    }

    private static void ExplodeNeighborBomb(RaycastHit2D hit, Vector2 dir)
    {
        Bomb bomb = hit.collider.GetComponent<Bomb>();
        if (bomb != null)
        {
            if (!bomb.exploaded)
            {
                hit.collider.GetComponent<Animator>().enabled = false;
                bomb.Explode(-dir);
            }
        }
    }

    void SpawnBonus(Vector3 position)
    {
        bool propability = Random.Range(0, 99) < bonusDropChance;
        if (propability)
        {
            int index = Random.Range(0, bonusArray.Length);
            Instantiate(bonusArray[index], new Vector3(position.x + 0.5f, position.y + 0.5f),
                                                        transform.rotation);
        }
    }

    IEnumerator ExplosionLine(Vector2 direction, float distance)
    {
        for (int i = 1; i <= distance; i++)
        {
            Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y) + direction * i;
            GameObject explosionVFX = Instantiate(explosion, spawnPos, transform.rotation);

            bool damageableFound = Physics2D.OverlapCircle(spawnPos, 0.45f, damageables);
            if (damageableFound)
            {
                Physics2D.OverlapCircle(spawnPos, 0.45f, damageables).GetComponent<IDamageable>().Die();
            }

            Destroy(explosionVFX, 0.5f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void SetPlayerReference(Player playerOrig)
    {
        player = playerOrig;
    }

    void OnTriggerExit2D()
    {
        //GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}