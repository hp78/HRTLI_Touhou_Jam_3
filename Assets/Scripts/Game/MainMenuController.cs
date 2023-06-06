using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;

using Luminosity.IO;

public class MainMenuController : MonoBehaviour
{
    // States
    [Space(15)]
    public BoolVar bvInLoading;
    public BoolVar bvGameOver;
    public BoolVar bvGamePaused;
    public BoolVar bvGameInCutscene;

    // GameStart options
    [Space(15)]
    public IntVar  ivPlayerCount;
    public BoolVar bvIsPlayer1Alive;
    public BoolVar bvIsPlayer2Alive;

    // Next Level
    [Space(15)]
    public StringVar svNextSceneName;
    public string nextLevelSceneName;
    public LoadingScreenController loadingScreenController;

    [Space(15)]
    public EventSystem eventSystem;
    public IntVar ivContCount;

    [Space(10)]
    public GameObject controlBtn;
    [Space(5)]
    public GameObject player1KeyboardButtons;
    public GameObject player1GamepadButtons;
    public GameObject player2KeyboardButtons;
    public GameObject player2GamepadButtons;
    [Space(5)]
    public GameObject player1SectionBtn;
    public GameObject player2SectionBtn;
    public GameObject player3SectionBtn;
    public GameObject player4SectionBtn;
    [Space(5)]
    public GameObject player1FirstBtn;
    public GameObject player2FirstBtn;
    public GameObject player3FirstBtn;
    public GameObject player4FirstBtn;
    [Space(5)]
    public GameObject player1KeyboardGamepadBtn;
    public GameObject player2KeyboardGamepadBtn;
    public GameObject player3KeyboardGamepadBtn;
    public GameObject player4KeyboardGamepadBtn;

    bool isPlayer1Gamepad = false;
    bool isPlayer2Gamepad = false;
    bool isPlayer3Gamepad = false;
    bool isPlayer4Gamepad = false;

    [Space(10)]
    public GameObject playersBtn;
    public Text playersTxt;
    bool isSetControl = false;


    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        bvInLoading.data = false;
        bvGameOver.data = false;
        bvGamePaused.data = false;
        bvGameInCutscene.data = false;
        ivContCount.data = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventSystem.currentSelectedGameObject == playersBtn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnePlayer();
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                TwoPlayer();
        }
        else if (eventSystem.currentSelectedGameObject == player1KeyboardGamepadBtn && Input.GetKeyDown(KeyCode.Return))
        {
            //TogglePlayer1Gamepad();
        }
        else if (eventSystem.currentSelectedGameObject == player1SectionBtn)
        {
            isSetControl = true;
            eventSystem.SetSelectedGameObject(player1FirstBtn);
            player1SectionBtn.SetActive(false);
            player2SectionBtn.SetActive(true);
        }
        else if (eventSystem.currentSelectedGameObject == player2SectionBtn)
        {
            isSetControl = true;
            eventSystem.SetSelectedGameObject(player2FirstBtn);
            player1SectionBtn.SetActive(true);
            player2SectionBtn.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSetControl)
                eventSystem.SetSelectedGameObject(controlBtn);
            else
                BackPlayGame();
        }
    }

    public void PlayGame()
    {
        animator.CrossFade("MM_Play1", 0.6f);

        ivPlayerCount.data = 1;
        bvIsPlayer1Alive.data = true;
        bvIsPlayer2Alive.data = false;
    }

    public void BackPlayGame()
    {
        animator.CrossFade("MM_Main", 0.6f);
    }

    public void ConfigControl()
    {    
        eventSystem.SetSelectedGameObject(player1SectionBtn);
    }

    public void StartGame()
    {
        // don't change level while a level transition in progress
        if (bvInLoading.data)
        {
            Debug.Log("ChangeLevel(" + nextLevelSceneName + ") called while in level change");
            return;
        }

        // Turn on loading anim
        loadingScreenController.TransitionToLoading();

        // Loads the next scene in the background
        svNextSceneName.data = nextLevelSceneName;
        bvInLoading.data = true;
        StartCoroutine(LoadSceneInAsync());
    }

    IEnumerator LoadSceneInAsync()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        //#if UNITY_EDITOR
        //SceneManager.LoadScene(svNextSceneName.data);
        //yield return 0;
        //#else

        Time.timeScale = 0.0f;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(svNextSceneName.data);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(2.0f);

        // Wait until the asynchronous scene fully loads
        while (asyncLoad.progress < 0.9f)
        {
            yield return new WaitForSecondsRealtime(1.0f);
        }
        asyncLoad.allowSceneActivation = true;
        Time.timeScale = 1.0f;
        //#endif
    }
    public void OnePlayer()
    {
        ivPlayerCount.data = 1;
        bvIsPlayer1Alive.data = true;
        bvIsPlayer2Alive.data = false;
        playersTxt.text = "players: 1";

        animator.CrossFade("MM_Play1", 0.6f);
    }

    public void TwoPlayer()
    {
        ivPlayerCount.data = 2;
        bvIsPlayer1Alive.data = true;
        bvIsPlayer2Alive.data = true;
        playersTxt.text = "players: 2";

        animator.CrossFade("MM_Play2", 0.6f);
    }

    public void ShowCredits()
    {
        animator.CrossFade("MM_Credit", 0.6f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TogglePlayer1Gamepad()
    {
        isPlayer1Gamepad = !isPlayer1Gamepad;
        player1KeyboardButtons.SetActive(!isPlayer1Gamepad);
        player1GamepadButtons.SetActive(isPlayer1Gamepad);
        string btnString = "keyboard";

        if (isPlayer1Gamepad)
        {
            btnString = "gamepad";
        }

        player1KeyboardGamepadBtn.GetComponentInChildren<Text>().text = btnString;
    }

    public void TogglePlayer2Gamepad()
    {
        isPlayer2Gamepad = !isPlayer2Gamepad;
        player2KeyboardButtons.SetActive(!isPlayer2Gamepad);
        player2GamepadButtons.SetActive(isPlayer2Gamepad);
        string btnString = "keyboard";

        if (isPlayer2Gamepad)
        {
            btnString = "gamepad";
        }

        player2KeyboardGamepadBtn.GetComponentInChildren<Text>().text = btnString;
    }
}

