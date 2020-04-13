using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UDSFMainMenu : MonoBehaviour
{
    public UDSFCanvas Canvas;

    public int SceneIndex;

    public void LoadScene()
    {
        Canvas.ShowLoadScreen(1f);
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneIndex, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) yield return null;

        Canvas.HideLoadScreen(1f);

        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.UnloadSceneAsync(currentScene);
    }
}
