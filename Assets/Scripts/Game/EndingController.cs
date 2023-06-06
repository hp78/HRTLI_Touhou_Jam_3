using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    public LoadingScreenController loadingScreenController;
    public StringVar svNextSceneName;
    public BoolVar bvInLoading;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMain()
    {
        // Turn on loading anim
        loadingScreenController.TransitionToLoading();

            // Loads the next scene in the background
            svNextSceneName.data = "MainMenu";
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
