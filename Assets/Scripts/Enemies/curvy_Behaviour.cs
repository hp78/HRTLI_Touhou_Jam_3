using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class curvy_Behaviour : AimedFire_Behaviour
{
    public BulletPattern pattern2;
    public GameObject empty;
    public List<GameObject> empties;
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shoot()
    {
        GameObject emptything = Instantiate(empty);
        emptything.transform.position = transform.position;

        empties.Add(emptything);

        BulletFactory.instance.ShootAt(emptything.transform, Player.transform, pattern, false);
        BulletFactory.instance.ShootAt(emptything.transform, Player.transform, pattern2, false);
    }

    public override void Kill()
    {
        gameObject.SetActive(false);
        foreach (GameObject curr in empties)
        {
            Destroy(curr);
        }
    }
}
