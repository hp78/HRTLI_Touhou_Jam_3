using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satori : BossBase
{

    public BulletPattern pattern1a;
    public BulletPattern pattern1a2;
    public BulletPattern pattern1b;
    public BulletPattern pattern1b2;
    public float phase1FireCD;

    public Transform[] waypoints1;
    public float phase1Speed;


    public BulletPattern pattern2a;
    public BulletPattern pattern2b;
    public BulletPattern pattern2c;
    public float phase2FireCD;
    public Transform waypoint2;
    public Transform[] waypoint2S;
    public float phase2Speed;

    public BulletComplexPattern pattern3a;
    public BulletComplexPattern pattern3b;
    public float phase3FireCD;



    float fireCD;

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
                if (currLives <= 0)
                {
                    if (bossState != BossState.DEAD)
                        StartCoroutine(DeathSeq(2));
                    bossState = BossState.DEAD;
                    if(!dead)
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
            if (!shot)
            {
                shot = true;
                BulletFactory.instance.ShootAt(this.transform, player.transform, pattern1a, false);
                BulletFactory.instance.ShootAt(this.transform, player.transform, pattern1b2, false);
                BulletFactory.instance.ShootAt(this.transform, player.transform, pattern1a2, false);
                BulletFactory.instance.ShootAt(this.transform, player.transform, pattern1b, false);

            }
            fireCD -= Time.deltaTime;
            if (fireCD < 0.0f)
            {
                fireCD = phase1FireCD;
                ++targetPt;
                if (targetPt == waypoints1.Length)
                    targetPt = 0;
                shot = false;
            }

        }
    }

    void Phase3()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoint2.position, phase1Speed * Time.deltaTime);
        if (!shot)
        {
            Vector3 facing = player.transform.position - transform.position;
            float playerAngle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
            shot = true;
            float ran = Random.Range(-0, 0);

            BulletFactory.instance.ComplexShoot(this.transform, playerAngle +ran+90f, pattern3a, false);
            BulletFactory.instance.ComplexShoot(this.transform, playerAngle +ran+ 90f, pattern3b, false);
        }
            //BulletFactory.instance.ComplexShoot(this.transform, playerAngle - 90f, pattern2b, false);
        fireCD -= Time.deltaTime;
        if (fireCD < 0.0f)
        {
            fireCD = phase3FireCD;
   
            shot = false;
        }
    }

    void Phase2()
    {

        this.transform.position = Vector2.MoveTowards(this.transform.position, waypoint2S[targetPt].position, phase2Speed * Time.deltaTime);

        if (Vector2.Distance(this.transform.position, waypoint2S[targetPt].position) < 0.1f)
        {
            ++targetPt;
            if (targetPt == waypoint2S.Length)
                targetPt = 0;
        }

        if (!shot)
        {
            shot = true;
            var temp1 = Instantiate(spawnTarget, waypoint2.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3),0.0f), Quaternion.identity);
            var temp2 = Instantiate(spawnTarget, waypoint2.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3),0.0f), Quaternion.identity);
            var temp3 = Instantiate(spawnTarget, waypoint2.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3),0.0f), Quaternion.identity);


            BulletFactory.instance.Shoot(temp1.transform, 0.0f, pattern2a, false);
            BulletFactory.instance.Shoot(temp2.transform, 0.0f, pattern2b, false);
            BulletFactory.instance.Shoot(temp3.transform, 0.0f, pattern2c, false);

            Destroy(temp1.gameObject, 3f);
            Destroy(temp2.gameObject, 4f);
            Destroy(temp3.gameObject, 5f);
                

        }
        fireCD -= Time.deltaTime;
        if (fireCD < 0.0f)
        {
            fireCD = phase2FireCD;
            shot = false;
        }

        
    }
}
