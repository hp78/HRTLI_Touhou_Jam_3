using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbdude_Behaviour : Enemy_base
{
    public List<GameObject> orbs;
    // Start is called before the first frame update

    // Update is called once per frame
    public override void Shoot()
    {
        foreach (GameObject curr in orbs)
        {
            curr.SendMessage("Pew",pattern);
        }
    }
}
