using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRotation_Behaviour : Enemy_base
{
    public float firerate = 0;

    protected override void Start()
    {
        if(firerate == 0)
        {
            firerate = 0.25f;
        }
    }

    public override void Shoot()
    {
        if(gameObject.activeInHierarchy)
        StartCoroutine(pewpewShoot());
    }

    IEnumerator pewpewShoot()
    {

        while(gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(firerate);
            BulletFactory.instance.Shoot(transform, transform.rotation.eulerAngles.z + 180f, pattern, false);
        }
        
    }
}
