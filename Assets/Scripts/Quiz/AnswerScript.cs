using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    [SerializeField] private GameObject correctIndicator;
    [SerializeField] private GameObject wrongIndicator;
    private bool animationDone = true;

    private void Start() {
        animationDone = true;
    }

    public void Answer(){
        if (!animationDone) return;
        StartCoroutine(AnimateChoice());
    }

    private IEnumerator AnimateChoice() {
        animationDone = false;
        if(isCorrect){
            wrongIndicator.SetActive(false);
            correctIndicator.SetActive(true);
            quizManager.robotAnimator.SetBool("isCorrect", true);

            yield return new WaitForSeconds(.85f);

            correctIndicator.SetActive(false);
            quizManager.robotAnimator.SetBool("isCorrect", false);
            animationDone = true;
            quizManager.Correct();
        }else{
            correctIndicator.SetActive(false);
            wrongIndicator.SetActive(true);
            quizManager.robotAnimator.SetBool("isWrong", true);

            yield return new WaitForSeconds(.85f);

            quizManager.robotAnimator.SetBool("isWrong", false);
            wrongIndicator.SetActive(false);
            animationDone = true;
            quizManager.Wrong();
        }
    }
}
