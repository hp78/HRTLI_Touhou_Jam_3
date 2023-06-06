using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shikieiki : BossBase
{

    public BulletPattern pattern1a;
    public BulletPattern pattern1b;

    public float phase1FireCD;
    float fireCD;
    public float phase1Speed;

    public BulletPattern pattern2;
    public Transform laser;

    public float phase2FireCD;


    public BulletPattern pattern3aR;
    public BulletPattern pattern3aL;
    public BulletPattern pattern3b;
    public BulletPattern pattern3c;
    public GameObject[] familiars;
    public Transform target;
    public float phase3Speed;
    public float phase3FireCD;


    public Transform[] waypoints1;
    public Transform waypoints2;
    public Transform[] waypoints3;


    int targetPt;

    bool shot;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        shot = false;
        fireCD = phase1FireCD;

    }

    // Update is called once per frame
    void Update()
    {

        if (bossState == BossState.SPELL)
        {
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
        }
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
                if (currLives == 0)
                {
                    if (bossState != BossState.DEAD)
                        StartCoroutine(DeathSeq(0));
                    bossState = BossState.DEAD;
                    if (!dead)
                        sprite.enabled = !sprite.enabled;
                }
                else
                {
                    bossState = BossState.INVUL;
                    currentHealth = maxHealth;
                    boxCol.enabled = false;
                    fireCD = 1f;

                }
            }
            //Debug.Log("HIT");
            Destroy(collision.gameObject);
        }

    }

    void Phase1()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints1[targetPt].position, phase1Speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoints1[targetPt].position) < 0.1f)
        {
            if(!shot)
            {
                var temp = Instantiate(spawnTarget, player.transform.position,Quaternion.identity);
                shot = true;
                BulletFactory.instance.ShootAt(this.transform, temp.transform, pattern1a, false);
                BulletFactory.instance.ShootAt(this.transform, temp.transform, pattern1b, false);
                Destroy(temp, 5f);
            }
            fireCD -= Time.deltaTime;
            if(fireCD < 0.0f)
            {
                fireCD = phase1FireCD;
                ++targetPt;
                if (targetPt == waypoints1.Length)
                    targetPt = 0;
                shot = false;
            }
        }
    }

    void Phase2()
    {

        transform.position = Vector2.MoveTowards(transform.position, waypoints2.position, 2 * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoints2.position) < 0.01f)
        {
            if (!shot)
            {
                var temp = Instantiate(laser, transform.position, Quaternion.identity);
                BulletFactory.instance.Shoot(this.transform, 0.0f, pattern2, false);
                shot = true;


            }
            fireCD -= Time.deltaTime;
            if (fireCD < 0.0f)
            {
                fireCD = phase2FireCD;
                shot = false;

            }
            else if(fireCD < 2.0f)
                chargeUp.Play();


        }

    }
    void Phase3()
    {
        foreach (GameObject item in familiars)
            item.SetActive(true);
        transform.position = Vector2.MoveTowards(transform.position, waypoints3[targetPt].position, phase3Speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoints3[targetPt].position) < 0.1f)
        {
            if (!shot)
            {
                BulletFactory.instance.Shoot(familiars[0].transform, 0.0f, pattern3aR, false);
                BulletFactory.instance.Shoot(familiars[1].transform, 0.0f, pattern3aL, false);
                BulletFactory.instance.ShootAt(this.transform, target.transform, pattern3b, false);

                shot = true;
            }
            fireCD -= Time.deltaTime;
            if (fireCD < 0.0f)
            {
                fireCD = phase3FireCD;
                ++targetPt;
                if (targetPt == waypoints3.Length)
                    targetPt = 0;
                shot = false;
                BulletFactory.instance.ShootAt(this.transform, target.transform, pattern3c, false);

            }
            else if (fireCD < 3.0f)
                chargeUp.Play();


        }
    }
}

