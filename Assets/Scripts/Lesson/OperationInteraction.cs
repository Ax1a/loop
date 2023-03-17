using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class OperationInteraction : MonoBehaviour
{
    public TextMeshProUGUI output;
    public TMP_InputField numInput_1;
    public TMP_InputField numInput_2;
    public TMP_InputField str1;
    public TMP_InputField str2;
    public TextMeshProUGUI concatOutput;
    public TMP_Dropdown operationDropdown;
    string _playerName;
    // private int num1;
    // private int num2;
    // private char operation;
    // private int correctAnswer = 59;

    void Start()
    {
        _playerName = DataManager.GetPlayerName();
        int correctAnswer;
        if (output != null)
        {
            if (Int32.TryParse(output.text, out correctAnswer))
            {
                GenerateRandomNum(correctAnswer);

            }
        }

    }
    public void CheckAnswer()
    {
        int userNum1;
        int userNum2;
        int correctAnswer;

        if (Int32.TryParse(numInput_1.text, out userNum1) && Int32.TryParse(numInput_2.text, out userNum2) && Int32.TryParse(output.text, out correctAnswer))
        {
            switch (operationDropdown.value)
            {
                case 0: // addition
                    if (userNum1 + userNum2 == correctAnswer)
                    {
                        NPCCall("Great!!");
                        //generate new correct answer
                        GenerateRandomNum(correctAnswer);

                        Debug.Log("Correct");
                    }
                    else
                    {
                        Debug.Log("Inorrect");
                        NPCCall("I got a wrong answer...");

                    }
                    break;
                case 1: // subtraction
                    if (userNum1 - userNum2 == correctAnswer)
                    {
                        NPCCall("Great!!");
                        //generate new correct answer
                        GenerateRandomNum(correctAnswer);

                    }
                    else
                    {
                        NPCCall("I got a wrong answer...");
                    }
                    break;
                case 2: // multiplication
                    if (userNum1 * userNum2 == correctAnswer)
                    {

                        NPCCall("Great!!");
                    }
                    else
                    {
                        NPCCall("I got a wrong answer...");
                    }
                    break;
                case 3: // division

                    if (userNum1 / userNum2 == correctAnswer)
                    {
                        NPCCall("Great!!");
                    }
                    else
                    {
                        NPCCall("I got a wrong answer...");
                    }
                    break;
                case 4: // modulus
                    if (userNum2 != 0 && userNum1 % userNum2 == correctAnswer)
                    {
                        NPCCall("Great!!");
                    }
                    else
                    {
                        NPCCall("I got a wrong answer...");
                    }
                    break;
            }

        }
        else
        {
            NPCCall("Oh! It only accepts numeric values! Try again...");
        }
    }

    public void Concatenation()
    {
        string input1 = str1.text;
        string input2 = str2.text;
        bool isEmpty = false;

        if (input1 == "")
        {
            Debug.Log("Input is empty");
            isEmpty = true;

        }
        if (input2 == "")
        {
            Debug.Log("Input is empty");
            isEmpty = true;
        }

        if (!isEmpty)
        {
            Debug.Log(input1 + input2);
            concatOutput.text = input1 + input2;
        }
        else
        {
            Debug.Log("Input is empty");

        }


    }
    void GenerateRandomNum(int correctAnswer)
    {
        correctAnswer = UnityEngine.Random.Range(1, 100);
        if (output != null)
        {
            output.text = correctAnswer.ToString();
        }
    }
    void NPCCall(string message)
    {
        NPCDialogue.Instance.AddDialogue(message, _playerName);
        NPCDialogue.Instance.ShowDialogue();
    }
}
