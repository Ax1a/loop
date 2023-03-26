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

    private void OnEnable() {
        sequence = DOTween.Sequence();
        skipIndicator.SetActive(true);
        
        PlayAnimation();
    }

    private void Update() {
        // Skip cutscene
        if (Input.GetKeyDown(InputManager.Instance.skip)) {
            PlayTransition();
        }
        
        if (_transitionCG != null && _transitionCG.alpha == 1) {
            LoadScene();
        }
    }

    public void LoadScene() {
        SceneManager.LoadScene(sceneID);
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
