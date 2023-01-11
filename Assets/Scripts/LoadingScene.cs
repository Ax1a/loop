using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] GameObject LoadingScreen; 
    [SerializeField] Image LoadingBarFill;

    public void LoadScene(int sceneID) {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID) {
        LoadingBarFill.fillAmount = 0;
        LoadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        operation.allowSceneActivation = false;

        float progress = 0;

        while (!operation.isDone) {
            progress = Mathf.MoveTowards(progress, operation.progress, Time.deltaTime);
            LoadingBarFill.fillAmount = progress;

            if (LoadingBarFill.fillAmount >= 0.9f) {
                LoadingBarFill.fillAmount = 1;
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        // LoadingScreen.SetActive(true);

        // while (!operation.isDone) {
        //     float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
        //     LoadingBarFill.fillAmount = progressValue;

            // yield return null;
        // }
        // LoadingScreen.SetActive(false);
    }
}
