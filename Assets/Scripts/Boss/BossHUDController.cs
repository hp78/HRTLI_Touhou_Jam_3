using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHUDController : MonoBehaviour
{
    public Animator animator;

    public Canvas canvas;
    public Image healthBar;
    public Image portrait;
    public Image portraitBack;
    public Text spellName;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetBossPortrait(Sprite nPortrait)
    {
        portrait.sprite = nPortrait;
        portraitBack.sprite = nPortrait;
    }

    public void SetSpellCardName(string nName)
    {
        spellName.text = nName;
    }

    public void DoCutIn()
    {
        canvas.enabled = true;
        animator.CrossFade("cut-in", 0.1f);
    }

    public Image GetHealthBar()
    {
        return healthBar;
    }
}
