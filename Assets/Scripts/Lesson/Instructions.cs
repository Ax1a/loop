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
    void Start()
    {
        _playerName = DataManager.GetPlayerName();
    }
    void OnEnable()
    {
        isActive = true;
        Instruction();
        LayoutRefresher.Instance.RefreshContentFitter((RectTransform) transform);
    }
    void OnDisable()
    {
        isActive = false;
    }
    public void Instruction()
    {
        if (isActive)
        {
            NPCCall(instruction);
        }
    }
    void NPCCall(string message)
    {
        NPCDialogue.Instance.AddDialogue(message, _playerName);
        NPCDialogue.Instance.ShowDialogue();
    }
}
