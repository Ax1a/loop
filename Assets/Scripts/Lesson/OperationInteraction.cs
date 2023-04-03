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
    public TMP_InputField inputToPrint;
    public TextMeshProUGUI concatOutput;
    public TMP_Dropdown operationDropdown;
    public TMP_Dropdown RelationDropdown;
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
                            NPCCall("Great job!! I got the right answers!");
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
                            NPCCall("Great job!! I got the right answers!");
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
                            NPCCall("Great job!! I got the right answers!");
                            //generate new correct answer
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
                            NPCCall("Great job!! I got the right answers!");
                            //generate new correct answer
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
                            NPCCall("Great job!! I got the right answers!");
                            //generate new correct answer
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
                NPCCall("Great! Seems like I understand the lesson");
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

    public void ClearRelationInputs()
    {
        numInput_1.text = "0";
        numInput_2.text = "0";
        output.text = "";
        inputToPrint.text = "";
    }
    public void CheckRelationAnswer()
    {
        bool result;
        float leftOperand;
        float rightOperand;

        int _currPts = LessonDragDropValidation.Instance._currPts;
        int ptsToWin = LessonDragDropValidation.Instance.ptsToWin;

        //Checks if blocks are drag in the right place
        if (_currPts >= ptsToWin)
        {
            if (float.TryParse(numInput_1.text, out leftOperand) && float.TryParse(numInput_2.text, out rightOperand))
            {
                switch (RelationDropdown.value)
                {
                    /*
                        0 = "<" 
                        1 = ">" 
                        2 = "<=" 
                        3 = ">="
                        4 = "=="
                        5 = "!="  
                    */
                    case 0:
                        result = leftOperand < rightOperand;
                        RelationResult(result);
                        break;
                    case 1:
                        result = leftOperand > rightOperand;
                        RelationResult(result);

                        break;
                    case 2:
                        result = leftOperand <= rightOperand;
                        RelationResult(result);

                        break;
                    case 3:
                        result = leftOperand >= rightOperand;
                        RelationResult(result);

                        break;
                    case 4:
                        result = leftOperand == rightOperand;
                        RelationResult(result);

                        break;
                    case 5:
                        result = leftOperand != rightOperand;
                        RelationResult(result);

                        break;
                    default:
                        throw new System.ArgumentException("Invalid");
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
    void RelationResult(bool result)
    {
        if (result)
        {
            output.text = inputToPrint.text;
            Debug.Log("Play first block");
            NPCCall("Great job!! I got the right answers!");
        }
        else
        {
            Debug.Log("Play second block");
            NPCCall("I got a wrong answer...");
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

