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
    private bool skipEnabled = false;
    private Coroutine loadScene;

    private void Awake() {
        loading = false;
        sceneLoaded = false;
        skipEnabled = false;
        _transitionCG = null;
        loadScene = StartCoroutine(LoadScene());
    }

    private IEnumerator Start() {
        yield return new WaitForSeconds(4f);
        sequence = DOTween.Sequence();
        skipIndicator.SetActive(true);
        skipEnabled = true;
        PlayAnimation();
    }

    private void LateUpdate() {
        // Skip cutscene
        if (Input.GetKeyDown(InputManager.Instance.skip) && skipEnabled) {
            PlayTransition();
        }
        
        if (_transitionCG == null) return;
        if (_transitionCG.alpha == 1 && !loading) {
            if (sceneLoaded) {
                SceneManager.UnloadSceneAsync(gameObject.scene);
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneID));
            }
            else {
                if (loadScene == null) loadScene = StartCoroutine(LoadScene());
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
    }

    public void PlayTransition() {
        transition.SetActive(true);
        _transitionCG = transition.GetComponent<CanvasGroup>();
        loading = false;
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
