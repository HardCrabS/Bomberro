using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] float speed = 100f;
    [SerializeField] Transform body;
    [SerializeField] Bomb bombPrefab;
    [SerializeField] GameObject diePanel;
    [SerializeField] Text bombCounterText;

    [Header("Bonus")]
    [SerializeField] float timeToShoot = 3;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject shield;

    [SerializeField] Timer timer;
    [SerializeField] Text bestTime;

    private Rigidbody2D myRb;
    private float distance = 2f;
    private int maxBombs = 1;
    private int currBombs = 0;
    Animator animator;
    // Use this for initialization
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (bombCounterText != null)
            bombCounterText.text = maxBombs + "/" + maxBombs;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer1();
        MovePlayer2();
        FlipSprite();
    }

    public float GetDistance()
    {
        return distance;
    }

    public void IncreaseDistance(float amount)
    {
        distance += amount;
    }

    void MovePlayer1()
    {
        if (gameObject.CompareTag("Player1"))
        {
            if (Input.GetButton("Horizontal") || Input.GetButtonDown("Horizontal"))
            {
                myRb.velocity = new Vector2(Input.GetAxis("Horizontal"), 0).normalized * speed * Time.deltaTime;
            }
            else if (Input.GetButton("Vertical") || Input.GetButtonDown("Vertical"))
            {
                myRb.velocity = new Vector2(0, Input.GetAxis("Vertical")).normalized * speed * Time.deltaTime;
            }
            else
            {
                myRb.velocity = Vector2.zero;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                PlaceBomb();
            }
        }
    }

    void MovePlayer2()
    {
        if (gameObject.CompareTag("Player2"))
        {
            if (Input.GetButton("HorizArrow"))
            {
                myRb.velocity = new Vector2(Input.GetAxis("HorizArrow"), 0).normalized * speed * Time.deltaTime;
            }
            else if (Input.GetButton("VerticArrow"))
            {
                myRb.velocity = new Vector2(0, Input.GetAxis("VerticArrow")).normalized * speed * Time.deltaTime;
            }
            else
            {
                myRb.velocity = Vector2.zero;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                PlaceBomb();
            }
        }
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRb.velocity.x) >= Mathf.Epsilon;
        bool hasVerticalSpeed = Mathf.Abs(myRb.velocity.y) >= Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            animator.SetBool("RunningDown", false);
            animator.SetBool("RunningUp", false);
            animator.SetBool("RunningX", true);
            body.localScale = new Vector2(Mathf.Sign(myRb.velocity.x), 1);
        }
        else
        {
            animator.SetBool("RunningX", false);
        }

        if (myRb.velocity.y > Mathf.Epsilon && hasVerticalSpeed)
        {
            animator.SetBool("RunningDown", false);
            animator.SetBool("RunningUp", true);
        }
        else if (myRb.velocity.y < -Mathf.Epsilon && hasVerticalSpeed)
        {
            animator.SetBool("RunningUp", false);
            animator.SetBool("RunningDown", true);
        }
        else
        {
            animator.SetBool("RunningUp", false);
            animator.SetBool("RunningDown", false);
        }
    }

    public void Die()
    {
        if (!shieldAcivated)
        {
            if (timer != null)
            {
                GameData gameData = GameData.gameData;
                float currTime = timer.GetBestTime();
                if (currTime > gameData.saveData.bestTime)
                {
                    gameData.saveData.bestTime = currTime;
                    gameData.Save();
                }
                bestTime.text = "Best time is: " + string.Format("{0:0.0}", gameData.saveData.bestTime);
            }

            //RewardedAdsButton rewardButton = null;
            if (diePanel != null)
            {
                diePanel.SetActive(true);
              //  rewardButton = diePanel.GetComponentInChildren<RewardedAdsButton>();
            }
            var pvpManager = FindObjectOfType<PVPManager>();
            if (pvpManager != null)
                pvpManager.DecreasePlayersCount(this);
            if (GameData.gameData != null)
                GameData.gameData.respawnPosition = transform.position;

            //if (rewardButton != null)
            //    rewardButton.SetPlayerReference(this.gameObject);
            gameObject.SetActive(false);//Destroy(gameObject);
        }
    }

    public IEnumerator StartShooting()
    {
        float timer = 0;
        while (timer < timeToShoot)
        {
            timer += .3f;
            GameObject bullet = Instantiate(projectilePrefab, transform.position, transform.rotation);
            if (myRb.velocity.magnitude > 0)
                bullet.GetComponent<Rigidbody2D>().velocity = myRb.velocity;
            else
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * speed * Time.deltaTime;

            Destroy(bullet, 5f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    bool shieldAcivated = false;
    public IEnumerator ActivateShield()
    {
        shield.SetActive(true);
        shieldAcivated = true;
        yield return new WaitForSeconds(3f);
        shield.SetActive(false);
        shieldAcivated = false;
    }

    public void IncreaseMaxBombs()
    {
        maxBombs += 1;
        if (bombCounterText != null)
            bombCounterText.text = maxBombs - currBombs + "/" + maxBombs;
    }

    public void IncreaseBombs()
    {
        currBombs += 1;
        if (bombCounterText != null)
            bombCounterText.text = maxBombs - currBombs + "/" + maxBombs;
    }

    public void DecreaseBombs()
    {
        currBombs -= 1;
        if (bombCounterText != null)
            bombCounterText.text = maxBombs - currBombs + "/" + maxBombs;
    }

    void PlaceBomb()
    {
        if (currBombs < maxBombs)
        {
            Bomb go = Instantiate(bombPrefab, transform.position, transform.rotation);
            go.SetPlayerReference(this);
        }
    }
}
