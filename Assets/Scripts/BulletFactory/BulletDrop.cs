using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDrop : MonoBehaviour 
{
    

    //
    public BulletPattern bulletPattern;

    public BulletPattern Collected()
    {
        //SoundManager.Instance.PickupSound();
        gameObject.SetActive(false);
        return bulletPattern;
    }
}
