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
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        LoadingScreen.SetActive(true);

        while (!operation.isDone) {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
        LoadingScreen.SetActive(false);
    }
}
