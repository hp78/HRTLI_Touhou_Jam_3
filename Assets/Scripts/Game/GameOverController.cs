using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    //
    public GameController gameController;
    Animator animator;

    //
    [Space(10)]
    public BoolVar bvGameOver;
    public IntVar ivPlayerCount;
    public BoolVar bvPlayer1Alive;
    public BoolVar bvPlayer2Alive;

    //
    [Space(10)]
    public IntVar bvContinues;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameController.hudController.UpdateCount(bvContinues.data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameOver()
    {
        animator.Play("Pause");
    }

    public void ContinueButton()
    {
        animator.Play("Unpause");
    }

    public void ContinueGame()
    {
        ++bvContinues.data;
        gameController.hudController.UpdateCount(bvContinues.data);
        bvGameOver.data = false;

        if(ivPlayerCount.data == 1)
        {
            bvPlayer1Alive.data = true;
        }
        else if (ivPlayerCount.data == 2)
        {
            bvPlayer1Alive.data = true;
            bvPlayer2Alive.data = true;
        }

        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        gameController.ChangeLevel("MainMenu");
    }
}
