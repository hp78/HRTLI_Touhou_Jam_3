using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseAngle : Enemy_base
{
    float random;

    bool startShot = false;

    protected override void Start()
    {
        random = Random.Range(0, 0.3f);
    }

    private void Update()
    {
        if(startShot && random > 0)
        {
            random -= Time.deltaTime;
        }

        if(startShot && random <0)
        {
            Pew();
            startShot = false;
        }
    }

    public override void Shoot()
    {
        startShot = true;
    }

    void Pew()
    {
        BulletFactory.instance.Shoot(transform, Random.Range(0, 180), pattern, false);
    }
}
