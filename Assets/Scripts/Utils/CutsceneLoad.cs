using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CutsceneLoad : MonoBehaviour
{
    [SerializeField] private int sceneID;
    [SerializeField] private GameObject skipIndicator;
    [SerializeField] private GameObject transition;
    [SerializeField] private CanvasGroup canvasGroup;
    private Sequence sequence;
    private CanvasGroup _transitionCG;
    private bool loading = false;
    private bool sceneLoaded = false;

    private void Awake() {
        StartCoroutine(LoadScene());
    }

    private void OnEnable() {
        loading = false;
        sceneLoaded = false;
        sequence = DOTween.Sequence();
        skipIndicator.SetActive(true);
        
        PlayAnimation();
    }

    private void Update() {
        // Skip cutscene
        if (Input.GetKeyDown(InputManager.Instance.skip)) {
            PlayTransition();
        }
        
        if (_transitionCG != null && _transitionCG.alpha == 1 && !loading) {
            if (sceneLoaded) {
                SceneManager.UnloadSceneAsync(gameObject.scene);
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneID));
            }
            else {
                StartCoroutine(LoadScene());
            }
        }
    }

    public IEnumerator LoadScene() {
        loading = true;
        var scene = SceneManager.GetSceneByBuildIndex(sceneID);
        if (!scene.isLoaded) {
            var operation = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Single);
            while (!operation.isDone) {
                yield return null;
            }
            sceneLoaded = true;
        }
        else {
            SceneManager.SetActiveScene(scene);
            sceneLoaded = true;
        }
    }

    public void PlayTransition() {
        transition.SetActive(true);
        _transitionCG = transition.GetComponent<CanvasGroup>();
    }

    private void PlayAnimation() {
        // Add a fade-in tween to the sequence
        sequence.Append(canvasGroup.DOFade(1, 1.5f));

        // Add a fade-out tween to the sequence
        sequence.Append(canvasGroup.DOFade(0, 1.5f));

        // Set the number of loops to be infinite
        sequence.SetLoops(-1);
    }
}
