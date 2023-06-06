using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionType : MonoBehaviour {

    public bool piercing;
    public bool explosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((this.tag == "PlayerBullet" && collision.tag == "Enemy") || (this.tag == "EnemyBullet" && collision.tag == "Player") || collision.tag == "Wall" || collision.tag == "Destructible")
        {
            if (explosion)
            {
                //ExplosiveBullet temp = GetComponent<ExplosiveBullet>();
                //if (temp)
                //    temp.Explode();
            }

            else if (!piercing)
            {
                Destroy(this.gameObject);
            }

           
        }
    }

}
