using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    [SerializeField] private GameObject correctIndicator;
    [SerializeField] private GameObject wrongIndicator;

    public void Answer(){
        if (quizManager.selectedAnswer) return;
        StartCoroutine(AnimateChoice());
    }

    private IEnumerator AnimateChoice() {
        quizManager.selectedAnswer = true;
        if(isCorrect){
            wrongIndicator.SetActive(false);
            correctIndicator.SetActive(true);
            quizManager.robotAnimator.SetBool("isCorrect", true);

            yield return new WaitForSeconds(.85f);

            correctIndicator.SetActive(false);
            quizManager.robotAnimator.SetBool("isCorrect", false);
            quizManager.selectedAnswer = false;
            quizManager.Correct();
        }else{
            correctIndicator.SetActive(false);
            wrongIndicator.SetActive(true);
            quizManager.robotAnimator.SetBool("isWrong", true);

            yield return new WaitForSeconds(.85f);

            quizManager.robotAnimator.SetBool("isWrong", false);
            wrongIndicator.SetActive(false);
            quizManager.selectedAnswer = false;
            quizManager.Wrong();
        }
    }
}
