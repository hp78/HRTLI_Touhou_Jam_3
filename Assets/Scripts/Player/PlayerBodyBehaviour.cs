using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyBehaviour : MonoBehaviour
{
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.GetComponentInParent<PlayerController>();

        if(playerController == null)
        {
            Debug.LogAssertion("PlayerBodyBehaviour/Start() - cannot ref PlayerController script from parent.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(playerController.bvPlayerIsAlive.data && collision.gameObject.CompareTag("EnemyBullet"))
        {
            collision.gameObject.GetComponent<BulletBehaviour>().SetDead();
            playerController.OnPlayerReceiveDmg();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerController.bvPlayerIsAlive.data && collision.gameObject.CompareTag("EnemyBullet"))
        {
            var temp = collision.gameObject.GetComponent<BulletBehaviour>();
            if (temp)
                temp.SetDead();
            
            playerController.OnPlayerReceiveDmg();
        }
    }
}
