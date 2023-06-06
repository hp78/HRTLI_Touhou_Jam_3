using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERIT FROM THIS DONT USE IT BY ITSELF

public class Enemy_base : MonoBehaviour
{
    SpriteRenderer s_renderer;

    public float movementSpeed = 1;      //How fast this dude moves
    public int health = 5;               //How tanky this dude is

    public GameObject particle;
    public GameObject corpsePrefab;
    public BulletPattern pattern;
    SpriteRenderer enemySprite;

    public bool flip = true;

    private void Awake()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        s_renderer = gameObject.GetComponent<SpriteRenderer>();
        if(!flip)
        {
            return;
        }

        if (transform.parent.parent.localScale.y != -1)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
        }

  
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //protected virtual void Movement()
    //{

    //}

    public virtual void Shoot()
    {
        //shoot at default direction which is bad (up)
        //spawn direction in angles
        //pattern is mah own one
        //isplayer or not
        //BulletFactory.instance.Shoot();

        //shoot at transform
        //BulletFactory.instance.ShootAt();
    }

    public virtual void Kill()
    {
        gameObject.SetActive(false);
    }

    void Death()
    {
        if(health <= 0)
        {
            //Change to corpse
            Instantiate(corpsePrefab, transform.position, transform.rotation);
            Instantiate(particle, transform.position, transform.rotation);
            gameObject.SetActive(false);
            SoundManager.instance.PlayDestroy();
        }
    }

    public void GetDamaged(int damageValue)
    {
        SoundManager.instance.PlayDamage();
        health -= damageValue;
        Death();

        if(gameObject.activeInHierarchy)
        StartCoroutine(DamageFlash());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet")
        {
            GetDamaged(1);
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DamageFlash()
    {
        Color halfColor = new Color(1, 0, 0, 0.5f);
        Color fullColor = new Color(0.5f, 0.5f, 0.5f, 1);

        enemySprite.color = halfColor;

        yield return new WaitForSeconds(0.05f);

        enemySprite.color = fullColor;

        yield return new WaitForSeconds(0.05f);

        enemySprite.color = new Color(1, 1, 1, 1);
    }

}
