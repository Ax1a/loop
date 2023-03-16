using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class OperationInteraction : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;
    public TextMeshProUGUI operationDropdown;

    private int num1;
    private int num2;
    private char operation;
    private int correctAnswer = 10;

    //     public Text questionText;
    // public InputField inputField1;
    // public InputField inputField2;
    // public Dropdown operationDropdown;
    // public Text feedbackText;

    // private int num1;
    // private int num2;
    // private int correctAnswer;

    void Start()
    {
        // GenerateQuestion();
    }

    public void CheckAnswer()
    {
        int userNum1;
        int userNum2;
        int opr;

        if (Int32.TryParse(inputField1.text, out userNum1) && Int32.TryParse(inputField2.text, out userNum2) && Int32.TryParse(operationDropdown.text, out opr))
        {
            switch (opr)
            {
                case 0: // addition
                    if (userNum1 + userNum2 == correctAnswer)
                    {
                        Debug.Log("Correct");

                    }
                    else
                    {
                        Debug.Log("Inorrect");
                    }
                    break;
                case 1: // subtraction
                    if (userNum1 - userNum2 == correctAnswer)
                    {
                        Debug.Log("Correct");

                    }
                    else
                    {
                        Debug.Log("Inorrect");
                    }
                    break;
                case 2: // multiplication
                    if (userNum1 * userNum2 == correctAnswer)
                    {
                        Debug.Log("Correct");


                    }
                    else
                    {
                        Debug.Log("Inorrect");
                    }
                    break;
                case 3: // division
                    if (userNum1 / userNum2 == correctAnswer)
                    {
                        Debug.Log("Correct");


                    }
                    else
                    {
                        Debug.Log("Inorrect");

                    }
                    break;
            }
        }
        else
        {
            Debug.Log("Invalid input. Please enter integers only.");
        }
    }

    // void GenerateQuestion()
    // {
    //     int num1 = 0; 
    //     int num2 = 0;
    //     int operationIndex= 0;

    //     switch (operationIndex)
    //     {
    //         case 0:
    //             operation = '+';
    //             correctAnswer = num1 + num2;
    //             break;
    //         case 1:
    //             operation = '-';
    //             correctAnswer = num1 - num2;
    //             break;
    //         case 2:
    //             operation = '*';
    //             correctAnswer = num1 * num2;
    //             break;
    //         case 3:
    //             operation = '/';
    //             correctAnswer = num1 / num2;
    //             break;
    //     }

    //     questionText.text = num1 + " " + operation + " " + num2 + " = ?";
    //     inputField.text = "";

    // }

}
