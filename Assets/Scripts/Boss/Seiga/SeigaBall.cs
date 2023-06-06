using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeigaBall : MonoBehaviour
{

    public SpriteRenderer sprite;
    public SpriteRenderer sprite2;
    CircleCollider2D col;
    public Transform player;
    public float chaseSpeed;
    // object's rotation
    public Quaternion rot;
    public float rotationSpeed;
    public bool on;
    bool grow;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        sprite2 = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
        rot = Quaternion.Euler(0.0f, 0.0f, rotationSpeed);
        sprite.color = new Color(1f, 1f, 1f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if(on)
        {
            if (sprite.color.a < 1.0f)
            {
                sprite.color = new Color(1f, 1f, 1f, sprite.color.a + 1f * Time.deltaTime);
                sprite2.color = new Color(1f, 1f, 1f, sprite.color.a + 1f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
                col.enabled = true;
            }

        }
        else
        {
            if(sprite.color.a >0.0f)
            {
                sprite.color = new Color(1f, 1f, 1f, sprite.color.a - 1f * Time.deltaTime);
                sprite2.color = new Color(1f, 1f, 1f, sprite.color.a - 1f * Time.deltaTime);
                col.enabled = false;
            }
        }

        if(grow)
        {
            sprite.transform.localScale += new Vector3(1f,1f,.1f) *Time.deltaTime;
            if (sprite.transform.localScale.x > 0.35f)
                grow = false;
        }
        else
        {
            sprite.transform.localScale -= new Vector3(1f, 1f, .1f) * Time.deltaTime;
            if (sprite.transform.localScale.x < 0.33f)
                grow = true ;
        }

        // while time is active, rotate the object
 
       this.transform.rotation = this.transform.rotation * rot;
    }
}
