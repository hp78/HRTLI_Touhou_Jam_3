
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // static ref
    public static GameController instance;

    // States
    [Space(10)]
    public BoolVar bvInLoading;
    public BoolVar bvGameOver;
    public BoolVar bvGamePaused;
    public BoolVar bvGameInCutscene;
    bool isChangingLevel = false;

    // GameStart options
    public IntVar ivPlayerCount;
    public BoolVar bvIsPlayer1Alive;
    public BoolVar bvIsPlayer2Alive;

    // Next Level
    [Space(10)]
    public StringVar svNextSceneName;
    public string nextLevelSceneName;

    // Gameplay Modules
    [Space(10)]
    public LoadingScreenController loadingScreenController;
    public PauseMenuController pauseMenuController;
    public HUDController hudController;
    public DialogueController dialogueController;
    public GameOverController gameOverController;
    public BossHUDController bossHUDController;

    // Use this for initialization
    void Start()
    {
        // Destroy any duplicate instances created
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        // Set inital states
        bvInLoading.data = true;
        bvGamePaused.data = false;
        bvGameInCutscene.data = false;
        bvGameOver.data = false;

        // Auto Find Safety
        AutoPrepGameplayScene();
    }

    #region Module Targeting Block

    // Targets all Gameplay Module Prefabs
    public int AutoPrepGameplayScene()
    {
        int returnVal = 0;
        returnVal += SetLoadingScreen();
        returnVal += SetPauseMenu();
        returnVal += SetHUD();
        returnVal += SetDialogue();

        return returnVal;
    }

    // Targets the loading screen in the scene
    int SetLoadingScreen()
    {
        if (loadingScreenController != null) return 0;

        GameObject goLoadingScreen = GameObject.Find("LoadingScreen");

        if(goLoadingScreen == null)
        {
            Debug.LogAssertion("No LoadingScreen object. Please drag a prefab in.");
        }
        else
        {
            LoadingScreenController nLoadingScreenController = goLoadingScreen.GetComponent<LoadingScreenController>();
            if (nLoadingScreenController == null)
            {
                Debug.LogAssertion("GameController/SetLoadingScreen() - Your fucking loading screen has no LoadingScreenController script.");
            }
            else
            {
                loadingScreenController = nLoadingScreenController;
                return 0;
            }
        }
        return 1;
    }

    // Targets the pause menu in the scene
    int SetPauseMenu()
    {
        if (pauseMenuController != null) return 0;

        GameObject goPauseMenu = GameObject.Find("PauseMenu");

        if (goPauseMenu == null)
        {
            Debug.LogAssertion("No PauseMenu object. Please drag a prefab in.");
        }
        else
        {
            PauseMenuController nPauseMenuController = goPauseMenu.GetComponent<PauseMenuController>();
            if (nPauseMenuController == null)
            {
                Debug.LogAssertion("GameController/SetPauseMenu() - Your fucking pause menu has no PauseMenuController script.");
            }
            else
            {
                pauseMenuController = nPauseMenuController;
                return 0;
            }
        }
        return 1;
    }

    // Targets the HUD in the scene
    int SetHUD()
    {
        if (hudController != null) return 0;

        GameObject goHUD = GameObject.Find("HUD");

        if (goHUD == null)
        {
            Debug.LogAssertion("No HUD object. Please drag a prefab in.");
        }
        else
        {
            HUDController nHudController = goHUD.GetComponent<HUDController>();
            if (nHudController == null)
            {
                Debug.LogAssertion("GameController/SetHUD() - Your fucking HUD has no HUDController script.");
            }
            else
            {
                hudController = nHudController;
                return 0;
            }
        }
        return 1;
    }

    // Targets the Dialogue in the scene
    int SetDialogue()
    {
        if (dialogueController != null) return 0;

        GameObject goDialogue = GameObject.Find("Dialogue");

        if (goDialogue == null)
        {
            Debug.LogAssertion("No Dialogue object. Please drag a prefab in.");
        }
        else
        {
            DialogueController nDialogueController = goDialogue.GetComponent<DialogueController>();
            if (nDialogueController == null)
            {
                Debug.LogAssertion("GameController/SetHUD() - Your fucking Dialogue has no DialogueController script.");
            }
            else
            {
                dialogueController = nDialogueController;
                return 0;
            }
        }
        return 1;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        // Nothing updates while game is changing level
        if (isChangingLevel)
        {
            return;
        }

        if(bvGameOver.data)
        {
            return;
        }

        //
        if (!(bvIsPlayer1Alive.data) && !(bvIsPlayer2Alive.data))
        {
            bvGameOver.data = true;

            //game over controller
            Time.timeScale = 0.0f;
            gameOverController.ShowGameOver();
        }

        // Escape for Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape) && !(bvGameInCutscene.data))
        {
            pauseMenuController.TogglePause(bvGamePaused.data);
        }
    }

    public void ChangeLevel(string levelName)
    {
        // don't change level while a level transition in progress
        if (bvInLoading.data)
        {
            Debug.Log("ChangeLevel(" + levelName +") called while in level change");
            return;
        }

        // Turn on loading anim
        loadingScreenController.TransitionToLoading();

        // Loads the next scene in the background
        svNextSceneName.data = levelName;
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
}
