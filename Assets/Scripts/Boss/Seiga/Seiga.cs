using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seiga : BossBase
{

    public BulletPattern pattern1a;
    public BulletPattern pattern1b;
    public SeigaBall[] herBalls;

    public float phase1FireCD;
    float fireCD;

    public BulletPattern pattern2;
    public Transform[] waypoints2;
    public float phase2Speed;
    public float phase2zombieSpeed;
        
    public Transform ZOMBIE;
    public Transform petZone;
    public float zombieDura;
     Quaternion rot;
    public float rotationSpeed;
    bool spin;
    bool zombieOn;
    int waypt;
    public GameObject target;
    public float phase2FireCD;


    public BulletPattern pattern3;
    public Transform cannon;
    
    public Transform[] waypoints3;
    public float phase3FireCD;


    bool shot;
    bool shotZombie;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        shot = false;
        fireCD = phase1FireCD;
        waypt = 0;
        rot = Quaternion.Euler(0.0f, 0.0f, rotationSpeed);

    }

    // Update is called once per frame
    void Update()
    {

        if (bossState == BossState.SPELL)
            switch (phaseNo)
            {
                case 1:
                    Phase1();
                    break;
                case 2:
                    Phase2();
                    break;
                case 3:
                    Phase3();
                    break;
                default: break;

            }
        health.fillAmount = (float)currentHealth / (float)maxHealth;


        if (bossState == BossState.INVUL)
            BossInvulMode();

        if (bossState == BossState.DECLARE)
        {
            Declare();
            health.fillAmount += .5f * Time.deltaTime;
        }

        EnemyFeedback();
        damagedFrame -= 0.5f;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet" && bossState != BossState.INVUL)
        {
            if (currentHealth > 0 && bossState != BossState.INVUL)
            {
                --currentHealth;

                sprite.color = dmgedColor;
                damagedFrame = 1.0f;
                PlayHit();
            }


            else
            {
                --currLives;
                if (currLives <= 0)
                {
                    if (bossState != BossState.DEAD)
                        StartCoroutine(DeathSeq(3));
                    bossState = BossState.DEAD;
                    if(!dead)
                    sprite.enabled = !sprite.enabled;

                }
                else
                {
                    bossState = BossState.INVUL;
                    currentHealth = maxHealth;
                    boxCol.enabled = false;
                    foreach (SeigaBall item in herBalls)
                    {
                        item.on = false;
                    }
                    fireCD = 1f;
                    StopAllCoroutines();
                    ZOMBIE.gameObject.SetActive(false);

                }
            }
            Debug.Log("HIT");
            Destroy(collision.gameObject);


        }

    }

    void Phase1()
    {

        if (!shot)
        {
            shot = true;
            StartCoroutine(Phase1Bullet());
            
        }
        fireCD -= Time.deltaTime;

        if (fireCD < 0.0f)
        {
            fireCD = phase1FireCD;
            shot = false;
        }

    }

    void Phase2()
    {
        ZOMBIE.gameObject.SetActive(true);
        if (!shot)
        {
            shot = true;
            shotZombie = true;
            target = Instantiate(spawnTarget, player.transform.position, Quaternion.identity);
            zombieOn = false;

            Vector3 dir = target.transform.position - ZOMBIE.transform.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            ZOMBIE.transform.rotation = Quaternion.AngleAxis(angle+120f, Vector3.forward);

        }

        if (shotZombie)
        {
            ZOMBIE.transform.position = Vector2.MoveTowards(ZOMBIE.transform.position, target.transform.position, phase2zombieSpeed * Time.deltaTime);
            if (Vector2.Distance(ZOMBIE.transform.position, target.transform.position) < 0.1f)
            {
                BulletFactory.instance.Shoot(ZOMBIE.transform, 0f, pattern2, false);
                BulletFactory.instance.Shoot(this.transform, 0f, pattern2, false);
                shotZombie = false;
                spin = true;
                Destroy(target.gameObject);
            }
        }

        if (zombieOn)
        {
            fireCD -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, waypoints2[waypt].position, phase2Speed * Time.deltaTime);
            ZOMBIE.transform.position = Vector2.MoveTowards(ZOMBIE.transform.position, petZone.transform.position, phase2zombieSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, waypoints2[waypt].position) < 0.1f)
            {
                ++waypt;
                if (waypt == waypoints2.Length)
                    waypt = 0;
            }

            Vector3 dir = player.position - ZOMBIE.transform.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;


            ZOMBIE.transform.rotation = Quaternion.AngleAxis(angle + 120f, Vector3.forward);


        }

        else
        {
            zombieDura -= Time.deltaTime;
            if(spin)
                ZOMBIE.transform.rotation = ZOMBIE.transform.rotation * rot;

        }

        if (zombieDura < 0.0f)
        {
            ZOMBIE.transform.position = Vector2.MoveTowards(ZOMBIE.transform.position, petZone.transform.position, phase2zombieSpeed * Time.deltaTime);
            if (Vector2.Distance(ZOMBIE.transform.position, petZone.transform.position) < 0.1f)
            {
                zombieOn = true;
                zombieDura = 5f;
                spin = false;

            }

        }

        if (fireCD < 0.0f)
        {
            fireCD = phase2FireCD;
            shot = false;
        }
  

    }

    IEnumerator Phase1Bullet()
    {
        foreach (SeigaBall item in herBalls)
        {
            item.on = true;
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-5f, 5f);
            item.transform.position = new Vector2(x, y);
        }
        yield return new WaitForSeconds(2f);
        BulletFactory.instance.Shoot(this.transform, 0f, pattern1a, false);
        BulletFactory.instance.Shoot(this.transform, 0f, pattern1b, false);
        yield return new WaitForSeconds(4f);
        BulletFactory.instance.Shoot(this.transform, 0f, pattern1a, false);
        BulletFactory.instance.Shoot(this.transform, 0f, pattern1b, false);
        yield return new WaitForSeconds(4f);
        BulletFactory.instance.Shoot(this.transform, 0f, pattern1a, false);
        BulletFactory.instance.Shoot(this.transform, 0f, pattern1b, false);
        yield return new WaitForSeconds(1f);

        foreach (SeigaBall item in herBalls)
        {
            item.on = false;
        }
        yield return 0;

    }

    void Phase3()
    {
        cannon.transform.position = Vector2.MoveTowards(cannon.transform.position, waypoints3[waypt].position, phase2Speed * Time.deltaTime);
        
        if (Vector2.Distance(cannon.transform.position, waypoints3[waypt].position) < 0.1f)
        {
            ++waypt;
            if (waypt == waypoints3.Length)
                waypt = 0;
        }

        if (!shot)
        {
            float ranX = Random.Range(-10f, 10f);

            

            shot = true;
             BulletFactory.instance.Shoot(cannon.transform, ranX, pattern3, false);
            BulletFactory.instance.Shoot(cannon.transform, ranX, pattern3, false);
            



        }
        fireCD -= Time.deltaTime;

        if (fireCD < 0.0f)
        {

            fireCD = phase3FireCD + Random.Range(-0.25f,0.25f);
            shot = false;
        }

    }
}
