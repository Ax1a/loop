using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    bool isActive;
    GameObject parentGameObject;
    [SerializeField] string instruction;
    string _playerName;
    public GameObject outputParent;

    void Start()
    {
        _playerName = DataManager.GetPlayerName();
        LayoutRefresher.Instance.RefreshContentFitter((RectTransform)transform);

    }
    void OnEnable()
    {
        isActive = true;
        Instruction();
        LayoutRefresher.Instance.RefreshContentFitter((RectTransform)transform);
    }
    void OnDisable()
    {
        isActive = false;
    }
    public void Instruction()
    {
        if (instruction != "")
        {
            if (isActive)
            {
                NPCCall(instruction);
            }
        }
    }
    void NPCCall(string message)
    {
        NPCDialogue.Instance.AddDialogue(message, _playerName);
        NPCDialogue.Instance.ShowDialogue();
    }

    public void ActivateConsoleOutput()
    {
        if (outputParent != null)
        {
            foreach (Transform childTransform in outputParent.transform)
            {
                GameObject childObject = childTransform.gameObject;
                childObject.SetActive(true);
            }
        }
    }
    public void ClearConsole()
    {
        if (outputParent != null)
        {
            foreach (Transform childTransform in outputParent.transform)
            {
                GameObject childObject = childTransform.gameObject;
                childObject.SetActive(false);
            }
        }
    }
}
