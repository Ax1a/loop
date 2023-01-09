using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quizControl : MonoBehaviour
{
    [SerializeField] GameObject quizContent;

    void Start()
    {
         
    }
    public void openQuiz(){
        quizContent.SetActive(true);
    }
}
