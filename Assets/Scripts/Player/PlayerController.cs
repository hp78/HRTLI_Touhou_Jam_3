using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;
public class PlayerController : MonoBehaviour
{
    //
    GameController gameController;

    //
    public BoolVar bvInLoading;
    public BoolVar bvGamePaused;
    public BoolVar bvGameInCutscene;



    //
    const float hitDuration = 3.0f;
    float lastHitDuration = 0.0f;

    //
    const float bombDuration = 2.0f;
    float lastBombDuration = 0.0f;

    //
    public BulletPattern bp;
    //
    [Space(10)]
    public BoolVar bvPlayer1IsAlive;
    public BoolVar bvPlayer2IsAlive;
    public IntVar ivPlayerCount;
    public BoolVar bvPlayerIsAlive;
    public PlayerID playerID;

    [Space(10)]
    public PlayerCartController cartController;
    public SpriteRenderer playerSprite;
    public Sprite aliveSprite;
    public Sprite deadSprite;

    [Space(10)]
    public Transform bulletSpawnPt;
    public BulletPattern playerBulletPattern;
    const float shootCooldown = 0.1f;
    float lastShootDuration = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(playerID == PlayerID.One)
        {
            bvPlayerIsAlive = bvPlayer1IsAlive;
        }
        else if(playerID == PlayerID.Two)
        {
            if(ivPlayerCount.data < 2)
            {
                bvPlayer2IsAlive.data = false;
                gameObject.SetActive(false);
            }
            bvPlayerIsAlive = bvPlayer2IsAlive;
            bvPlayerIsAlive.data = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        if(bvInLoading.data || bvGamePaused.data || bvGameInCutscene.data)
        {
            return;
        }


        lastBombDuration -= Time.deltaTime;
        lastHitDuration -= Time.deltaTime;
        UpdateInput();

        if (bvPlayerIsAlive.data)
        {
            AutoShoot();

            if(playerSprite.sprite == deadSprite)
            {
                playerSprite.sprite = aliveSprite;
            }
        }
    }

    void UpdateInput()
    {
        // Movement
        float hori = InputManager.GetAxis("Horizontal", playerID);
        float vert = InputManager.GetAxis("Vertical", playerID);
        Vector3 moveVector = new Vector3(hori, vert, 0);

        float speed = 6.0f;

        if(InputManager.GetButton("Focus", playerID) || InputManager.GetButton("FocusGP", playerID))
        {
            speed = 2.0f;
        }

        transform.position += moveVector.normalized * speed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.5f, 8.5f),
                        Mathf.Clamp(transform.position.y,-4.5f,4.5f), 
                        transform.position.z);
        cartController.SetInertia(hori, speed);

        // Shoot Corpse
        if (InputManager.GetButtonDown("Shoot", playerID) || InputManager.GetButtonDown("ShootGP", playerID))
        {
            Shoot();
        }

        // Bomb Corpse
        if (InputManager.GetButtonDown("Bomb", playerID) || InputManager.GetButtonDown("BombGP", playerID))
        {
            Bomb();
        }
    }

    void AutoShoot()
    {
        lastShootDuration -= Time.deltaTime;

        if(lastShootDuration < 0.0f)
        {
            BulletFactory.instance.Shoot(bulletSpawnPt, 0.0f, playerBulletPattern, true);
            lastShootDuration = shootCooldown;
        }
    }

    void Shoot()
    {
        ShootCorpse();
    }

    void Bomb()
    {
        BombCorpses();
    }

    public void OnPlayerReceiveDmg()
    {
        if(lastHitDuration < 0.0f)
        {
            EjectCorpses();
        }
    }

    IEnumerator DamageFlash()
    {
        Color blankColor = new Color(1, 1, 1, 0);
        Color halfColor = new Color(1, 1, 1, 0.5f);
        Color fullColor = new Color(1, 1, 1, 1);

        while (lastHitDuration > 0.2f)
        {
            playerSprite.color = blankColor;
            yield return new WaitForSeconds(0.05f);

            playerSprite.color = halfColor;
            yield return new WaitForSeconds(0.05f);

            playerSprite.color = blankColor;
            yield return new WaitForSeconds(0.05f);

            playerSprite.color = fullColor;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void EjectCorpses()
    {
        lastHitDuration = hitDuration;
        StartCoroutine(DamageFlash());

        if (cartController.EjectCorpses())
        {
            //
        }
        else
        {         
            bvPlayerIsAlive.data = false;
            playerSprite.sprite = deadSprite;

            if (ivPlayerCount.data == 1)
            {
                GameController.instance.bvGameOver.data = true;
                Invoke("Die", 0.1f);
            }
            else if (ivPlayerCount.data == 2)
            {
                if(!(bvPlayer1IsAlive.data) && !(bvPlayer2IsAlive.data))
                {
                    GameController.instance.bvGameOver.data = true;
                    Invoke("Die", 0.1f);
                }
            }
        }
    }

    void Die()
    {
        Time.timeScale = 0.0f;
        GameController.instance.gameOverController.ShowGameOver();
    }
    void ShootCorpse()
    {
        cartController.ShootCorpse();
    }
    void BombCorpses()
    {
        cartController.BombCorpses();
    }
}

