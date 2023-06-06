using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFire_Behaviour : Enemy_base
{
    public List<GameObject> lineObjects;

    public override void Shoot()
    {
        lineObjects[0].SendMessage("Pew", pattern);

        lineObjects[1].SendMessage("Pew", pattern);
        lineObjects[2].SendMessage("Pew", pattern);
    }
}
