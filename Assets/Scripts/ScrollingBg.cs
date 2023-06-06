using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBg : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public Transform[] BGs;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform item in BGs)
        {
            item.transform.position = Vector2.MoveTowards(item.transform.position, end.transform.position, speed * Time.deltaTime);
            if (item.transform.position == end.transform.position)
            {
                item.transform.position = start.position;
                item.transform.position = Vector2.MoveTowards(item.transform.position, end.transform.position, speed * Time.deltaTime);

            }


        }


    }
}
