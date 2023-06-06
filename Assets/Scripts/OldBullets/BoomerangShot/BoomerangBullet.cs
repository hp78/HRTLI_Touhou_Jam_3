using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : MonoBehaviour {


    // speed of rotation
    public float rotationSpeed;

    // object's rotation
    public Quaternion rot;

    public Transform[] childBullets;


	// Use this for initialization
	void Start () {


        if (transform.parent.tag == "PlayerBullet")
            foreach (Transform item in childBullets)
                item.tag = "PlayerBullet";
        else
            foreach (Transform item in childBullets)
                item.tag = "EnemyBullet";

        // set rotation speed
        rot = Quaternion.Euler(0.0f, 0.0f, rotationSpeed);

    }
	
	// Update is called once per frame
	void Update () {


        this.transform.rotation = this.transform.rotation * rot;

    }
}
