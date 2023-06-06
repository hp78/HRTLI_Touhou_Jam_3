using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShikiFam : MonoBehaviour
{
    // Start is called before the first frame update


    public Transform[] waypoint;
    public int targetPt;
    public float speed;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoint[targetPt].position, speed *Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoint[targetPt].position) < 0.1f)
        {

            if (targetPt == 0)
                targetPt = 1;
            else
                targetPt = 0;
        }

        speed += 0.25f * Time.deltaTime;

        if(speed > 8f)
        {
            speed = 3f;
        }
    }
}