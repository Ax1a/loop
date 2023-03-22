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
        int _currPts = LessonDragDropValidation.Instance._currPts;
        int ptsToWin = LessonDragDropValidation.Instance.ptsToWin;

        //Checks if blocks are drag in the right place
        if (_currPts >= ptsToWin)
        {
            //Checks if inputs are all numeric
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
                            GenerateRandomNum(correctAnswer);

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
                            GenerateRandomNum(correctAnswer);

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
                            GenerateRandomNum(correctAnswer);

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
        else
        {
            NPCCall("I think I have to drag the blocks in the right place...");

        }
    }

    public void Concatenation()
    {
        string input1 = str1.text;
        string input2 = str2.text;
        bool isEmpty = false;
        int _currPts = LessonDragDropValidation.Instance._currPts;
        int ptsToWin = LessonDragDropValidation.Instance.ptsToWin;

        //Checks if inputs are not empty
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

        //Checks if blocks are drag in the right place
        if (_currPts >= ptsToWin)
        {
            if (!isEmpty)
            {
                Debug.Log(input1 + input2);
                concatOutput.text = input1 + input2;
            }
            else
            {
                NPCCall("I must input a strings inside the text boxes...");
            }
        }
        else
        {
            NPCCall("I think I have to drag the blocks in the right place...");
        }
    }

    public void ClearNumeric()
    {
        //check if numInput_1 and numInput_2 is not null before accessing the text
        if (numInput_1?.text != null && numInput_2?.text != null)
        {
            numInput_1.text = "0";
            numInput_2.text = "0";
        }
    }

    public void ClearString()
    {
        //check if str1 and str2 is not null before accessing the text
        if (str1?.text != null && str2?.text != null)
        {
            str1.text = "";
            str2.text = "";
            concatOutput.text = "";
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
