using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    Animator animator;
    GameController gameController;
    public Text contCount;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogAssertion("HUDController/Start() - Animator component missing");

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (gameController == null)
            Debug.LogAssertion("HUDController/Start() - GameController ref not set");
    }

    public void UpdateCount(int val)
    {
        contCount.text = "" + val;
    }
}
