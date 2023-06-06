using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    Animator animator;
    GameController gameController;

    const float pauseLockDura = 0.5f;
    float pauseCooldown = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogAssertion("PauseMenuController/Start() - Animator component missing");

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if(gameController == null)
            Debug.LogAssertion("PauseMenuController/Start() - GameController ref not set");
    }

    void Update()
    {
        pauseCooldown -= Time.unscaledDeltaTime;
    }

    public void TogglePause(bool isPaused)
    {
        if(isPaused && pauseCooldown <= 0.0f)
        {
            UnpauseGame();
        }
        else if(pauseCooldown <= 0.0f)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // play pause menu entry anim
        animator.Play("Pause");
        gameController.bvGamePaused.data = true;
        pauseCooldown = pauseLockDura;
        Time.timeScale = 0.0f;

        
    }

    public void UnpauseGame()
    {
        // play pause menu exit anim
        animator.Play("Unpause");

        pauseCooldown = pauseLockDura;
    }

    // For animation to call
    void ResumeGameplay()
    {
        gameController.bvGamePaused.data = false;
        Time.timeScale = 1.0f;
    }

    public void OpenControls()
    {

    }

    public void ReturnToMainMenu()
    {
        SoundManager.instance.PlayMenu();
        gameController.ChangeLevel("MainMenu");
    }
}
