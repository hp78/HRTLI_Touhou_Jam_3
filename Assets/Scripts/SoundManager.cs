using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public AudioSource[] SFX;
    public AudioSource[] BGM;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayDamage()
    {
        SFX[0].pitch = 3;
        SFX[0].Play();
        SFX[0].pitch -= Random.Range(0, 1.5f);
    }

    public void PlayDestroy()
    {
        SFX[1].Play();
    }

    public void PlayMenu()
    {
        StopAllSounds();

        //PlayeMain menu
        BGM[0].Play();
    }

    public void PlayStageTheme()
    {
        StopAllSounds();

        BGM[1].Play();
    }

    public void PlayBossTheme()
    {
        StopAllSounds();
        BGM[2].Play();
    }

    public void PlayShikiTheme()
    {
        StopAllSounds();
        BGM[3].Play();
    }

    public void StopAllSounds()
    {
        foreach (AudioSource curr in BGM)
        {
            curr.Stop();
        }

        foreach (AudioSource curr in SFX)
        {
            curr.Stop();
        }
    }
}
