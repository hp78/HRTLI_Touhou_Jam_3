using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayShikiTheme : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SoundManager.instance.PlayShikiTheme();
        }
    }
}
