using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UVNF.Core.UI
{
    public class UVNFMainMenu : MonoBehaviour
    {
        public UVNFCanvas Canvas;

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

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Canvas.HideLoadScreen(1f);

            yield return new WaitForSeconds(1f);

            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}