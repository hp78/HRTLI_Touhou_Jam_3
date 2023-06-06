using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour {


    public float lifeTime;
   // public float speed;

    public BulletPattern explosivePattern;

    Rigidbody2D rigidbody2d;
    public bool isPlayer =  false;

	// Use this for initialization
	void Start () {

        rigidbody2d = GetComponent<Rigidbody2D>();
        // rigidbody2d.AddForce(new Vector2(speed, 0.0f));

        if (this.tag == "PlayerBullet")
            isPlayer = true;
        

    }
	
	// Update is called once per frame
	void Update () {
		
        
        if(lifeTime <0.0f)
        {
            Explode();
        }

        lifeTime -= Time.deltaTime;

	}

    public void Explode()
    {
        BulletFactory.instance.Shoot(this.transform, 0.0f, explosivePattern, isPlayer);
        Destroy(this.gameObject, 5f);
        this.gameObject.SetActive(false);
    }
}
