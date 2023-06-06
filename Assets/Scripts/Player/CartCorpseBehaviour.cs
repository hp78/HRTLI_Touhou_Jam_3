using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartCorpseBehaviour : MonoBehaviour
{
    public bool isAttached = false; // should be false
    //bool isEjected = false;
    bool isRocketMode = false;

    SpriteRenderer corpseSprite;
    SpriteRenderer corpseHighlightSprite;
    Color highlightColor;

    float corpseDestroyTimer = 10f;

    public float attachedAngle = 0.0f;
    const float attachedCooldown = 1.0f;
    float lastAttachedTime = 0.0f;
    float ejectedAngle = 0.0f;

    float currInertia = 0.0f;
    public CartCorpseBehaviour corpseChild;
    Vector3 corpseOffset = new Vector3(0.0f, 0.25f, -0.1f);

    [Space(10)]
    public BulletPattern bulletPattern;
    public float shootCooldown = 0.2f;
    float lastShootDuration = 0.0f;


    // bullet pattern

    // Start is called before the first frame update
    void Start()
    {
        corpseSprite = GetComponent<SpriteRenderer>();

        if (transform.childCount > 0 && transform.GetChild(0) != null)
        {
            //corpseChild = transform.GetChild(0).GetComponentInChildren<CartCorpseBehaviour>();
            corpseChild = null;
            corpseHighlightSprite = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
            highlightColor = corpseHighlightSprite.color;
        }

        StartCoroutine(CollectFlash());
    }

    // Update is called once per frame
    void Update()
    {
        GameController gc = GameController.instance;
        if (gc.bvGameInCutscene.data || gc.bvGamePaused.data || gc.bvInLoading.data || gc.bvGameOver.data)
            return;

        if(isAttached)
        {
            UpdateCorpseChild();
            UpdateShoot();
        }
        else if(isRocketMode)
        {
            UpdateRocket();
        }
        else
        {
            corpseDestroyTimer -= Time.deltaTime;
            UpdateCorpse();
            if(corpseDestroyTimer < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void UpdateCorpseChild()
    {
        if (corpseChild == null) return;

        Vector3 currEuRot = corpseChild.transform.localEulerAngles;
        corpseChild.transform.localEulerAngles = new Vector3(0, 0,
                                                Mathf.LerpAngle(currEuRot.z, attachedAngle + currInertia, 2.0f * Time.deltaTime));
    }

    void UpdateShoot()
    {
        lastShootDuration -= Time.deltaTime;

        if (lastShootDuration < 0.0f)
        {
            BulletFactory.instance.Shoot(transform, transform.rotation.eulerAngles.z, bulletPattern, true);
            lastShootDuration = shootCooldown;
        }
    }

    void UpdateRocket()
    {
        // 
        transform.position = transform.position + transform.right * Time.deltaTime * 10.0f;

        if(transform.position.x > 9.0f || transform.position.x < -9.0f)
        {
            isRocketMode = false;
            StartCoroutine(CollectFlash());
        }

        if (transform.position.y > 5.0f || transform.position.y < -5.0f)
        {
            isRocketMode = false;
            StartCoroutine(CollectFlash());
        }
    }

    void UpdateCorpse()
    {

        lastAttachedTime -= Time.deltaTime;
        transform.position -= transform.right * Time.deltaTime * 1.5f;

        if (lastAttachedTime < 0.0f)
            corpseSprite.color = new Color(corpseSprite.color.r, corpseSprite.color.g, corpseSprite.color.b, corpseDestroyTimer / 9.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttached) return;

        if (isRocketMode)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy_base>().GetDamaged(15);
                isRocketMode = false;
            }

            if (collision.gameObject.CompareTag("EnemyBullet"))
            {
                //print("ASDHASDHAHDDAh");
                collision.gameObject.GetComponent<BulletBehaviour>().SetDead();
            }

            return;
        }

        if (lastAttachedTime < 0.0f)
        {
            if (collision.gameObject.CompareTag("PlayerCart"))
            {
                if(collision.gameObject.GetComponent<PlayerCartController>().AttachCorpse(this))
                {
                    corpseDestroyTimer = 10f;
                    corpseSprite.flipX = true;
                    gameObject.tag = "CorpseCart";
                    isAttached = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRocketMode)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                isRocketMode = false;
                StartCoroutine(CollectFlash());
            }

            if (collision.gameObject.CompareTag("EnemyBullet"))
            {
                collision.gameObject.GetComponent<BulletBehaviour>().SetDead();
            }

            return;
        }
    }

    public void SetInertia(float val)
    {

        if (val > 0.1f)
        { 
            currInertia = val - 0.1f;
        }
        else if (val < -0.1f)
        {
            currInertia = val + 0.1f;
        }
        else
        {
            currInertia = -0.5f;
        }

        if (corpseChild != null)
        {
            corpseChild.SetInertia(currInertia);
        }
    }

    public bool EjectCorpses()
    {
        if(corpseChild != null)
        {
            corpseChild.EjectCorpses();
            corpseChild = null;
        } 

        transform.parent = null;
        
        isAttached = false;
        gameObject.tag = "CorpseCollectable";
        lastAttachedTime = attachedCooldown;

        StartCoroutine(DamageFlash());
        StartCoroutine(CollectFlash());

        ejectedAngle = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = new Vector3(0, 0, ejectedAngle);

        return true;
    }

    IEnumerator DamageFlash()
    {
        Color blankColor = new Color(0, 0, 0, 0);
        Color halfColor = new Color(1, 1, 1, 0.5f);
        Color fullColor = new Color(0.5f, 0.5f, 0.5f, 1);

        while (lastAttachedTime > 0.2f)
        {
            corpseSprite.color = blankColor;
            yield return new WaitForSeconds(0.05f);

            corpseSprite.color = halfColor;
            yield return new WaitForSeconds(0.05f);

            corpseSprite.color = blankColor;
            yield return new WaitForSeconds(0.05f);

            corpseSprite.color = fullColor;
            yield return new WaitForSeconds(0.05f);
        }
        corpseSprite.color = new Color(1, 1, 1, 1);
    }

    IEnumerator CollectFlash()
    {
        Color fullColor = new Color(0.8f, 1f, 0.8f, 0.9f);

        while (!isAttached)
        {
            corpseHighlightSprite.color = fullColor;
            yield return new WaitForSeconds(0.05f);

            corpseHighlightSprite.color = highlightColor;
            yield return new WaitForSeconds(0.95f);
        }
        corpseHighlightSprite.color = new Color(0, 0, 0, 0);
    }

    public CartCorpseBehaviour ShootCorpse()
    {
        CartCorpseBehaviour targetCorpse = null;
        targetCorpse = corpseChild;

        if (targetCorpse != null)
        {
            targetCorpse.transform.parent = transform.parent;
        }
        
        transform.parent = null;
        corpseChild = null;

        // 
        isRocketMode = true;
        isAttached = false;

        return targetCorpse;
    }

    public bool BombCorpses()
    {
        if (corpseChild == null)
        {
            return false;
        }

        corpseChild.BombCorpses();
        transform.parent = null;

        // 
        isRocketMode = true;
        isAttached = false;
        corpseChild = null;

        return true;
    }
    
    public void AttachCorpse(CartCorpseBehaviour corpse)
    {
        if (corpseChild == null)
        {
            attachedAngle = corpse.transform.eulerAngles.z;
            corpse.transform.parent = transform;
            corpseChild = corpse;
            corpseChild.transform.position = transform.position + corpseOffset;

        }
        else
        {
            corpseChild.AttachCorpse(corpse);
        }
    }

}
