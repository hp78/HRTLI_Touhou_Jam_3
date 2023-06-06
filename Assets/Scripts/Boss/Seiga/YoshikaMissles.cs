using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoshikaMissles : MonoBehaviour
{
    public float phase1FireCD;
    float fireCD;
    public BulletPattern pattern3;
    bool shot;
    int times;
    // Start is called before the first frame update
    void Start()
    {
        times = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shot && times <4)
        {
            shot = true;
            BulletFactory.instance.Shoot(this.transform, 0f, pattern3, false);

            ++times;
        }
        fireCD -= Time.deltaTime;

        if (fireCD < 0.0f)
        {
            fireCD = phase1FireCD;
            shot = false;
        }

    }
}
