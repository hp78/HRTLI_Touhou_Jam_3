using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenController : MonoBehaviour
{
    public bool isMainMenu = false;

    Animator animator;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogAssertion("LoadingScreenController/Start() - Animator component missing");


        if(isMainMenu)
        {

        }
        else
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
            if (gameController == null)
                Debug.LogAssertion("LoadingScreenController/Start() - GameController ref not set");
        }

        TransitionFromLoading();
    }

    //
    void TransitionFromLoading()
    {
        // animator play exit to loading screen
        //animator.Play("EnterLoading");
    }

    void EndLoading()
    {
        if (!isMainMenu)
        {
            gameController.bvInLoading.data = false;
        }
    }

    public void TransitionToLoading()
    {
        // Set tooltip

        // animator play entry to loading screen
        animator.Play("EnterLoading");
    }
}
