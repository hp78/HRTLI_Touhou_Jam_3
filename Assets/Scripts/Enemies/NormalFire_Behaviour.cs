using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFire_Behaviour : Enemy_base
{
    public bool randomise= false;
    float random;
    bool startShot = false;

    protected override void Start()
    {
        random = Random.Range(0, 0.5f);
    }
    private void Update()
    {
        if (startShot && random > 0)
        {
            random -= Time.deltaTime;
        }

        if (startShot && random < 0)
        {
            Pew();
            startShot = false;
        }
    }

    /// <summary>
    /// lerps to certain space (straight horizontal)
    /// </summary>
    //protected override void Movement()
    //{
    //    Vector2 posVector = transform.position;
    //    posVector.x = Mathf.Lerp(transform.position.x, xPosition, Time.deltaTime * lerpSpeed);

    //    transform.position = posVector;
    //}

    public override void Shoot()
    {
        if(randomise)
        {
            startShot = true;
            return;
        }

        BulletFactory.instance.Shoot(transform, 180, pattern, false);
    }

    void Pew()
    {
        BulletFactory.instance.Shoot(transform, 180, pattern, false);
    }

}
