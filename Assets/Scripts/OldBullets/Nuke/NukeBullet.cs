using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeBullet : MonoBehaviour
{


    public Transform bullet;
    public Transform explosion;

    public float explosionDelay;



    // Use this for initialization
    void Start()
    {


        if (transform.tag == "PlayerBullet")
        {
            bullet.tag = "PlayerBullet";
            explosion.tag = "PlayerBullet";
        }
        else
        {
            bullet.tag = "EnemyBullet";
            explosion.tag = "EnemyBullet";
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (explosionDelay < 0.0f)
        {
            bullet.gameObject.SetActive(false);
            explosion.gameObject.SetActive(true);
        }

        explosionDelay -= Time.deltaTime;
    }


}
