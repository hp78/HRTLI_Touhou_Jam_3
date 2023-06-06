/*********************************************************************************
All content ©2016 DigiPen Institute of Technology Singapore. All rights reserved.

Filename        BossLaserScript.cs
Author          Affirudin Bin Kamarudin (affirudin.k@digipen.edu)                                      

Description
    The behaviour of the giant laser projectile used by the final boss
Usage
    Used as a prefab, can be spawned as a projectile
*********************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class BossLaserScript : MonoBehaviour
{
    // Laser box collider
    public BoxCollider2D boxCollider;

    // Duration of laser life
    public float laserDuration = 5.0f;

    // Bool if laser is firing
    bool laserFiring;

    // Desired lasers final size
    public float desiredWidth = 50.0f;
    public float desiredLength = 10.0f;



    // Use this for initialization
    void Start ()
    {
        // Grab box collider
        boxCollider = GetComponent<BoxCollider2D>();

        // Set both collider and firing to false
        boxCollider.enabled = false;
        laserFiring = false;
 
	}
	
	// Update is called once per frame
	void Update ()
    {

        // if Time is moving
        if (Time.deltaTime > 0.0f)
        {
            // If laser has reached its end life
            if (laserDuration < 0.0f)
            {
                // set firing to false
                laserFiring = false;

                // Start shrinking the laser
                this.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - .5f, 1f);

                // Once the laser shrinks into a thin beam, destroy the laser
                if (this.transform.localScale.y < 0.0f)
                    Destroy(this.gameObject);
            }

            // else if the laser have not reaches the correct width, grow its width
            else if (transform.localScale.x < desiredWidth)
                this.transform.localScale = new Vector3(transform.localScale.x + .75f, 1.0f, 1f);

            // else if the laser have not reaches the correct length, grow its length
            else if (transform.localScale.y < desiredLength)
            {
                this.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 1.5f, 1f);
                
                // On both laser collider and firing to be true
                boxCollider.enabled = true;

                laserFiring = true;
            }

            // While its firing, shrink and grow the laser for extra realistic effect and decrease its firing time
            if (laserFiring)
            {
                this.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 1f, 1f);
                laserDuration -= Time.deltaTime;
            }
        }


    }
}
