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
    // [SerializeField] Sprite syntaxImage;
    // public Image img;
    void Start()
    {
        _playerName = DataManager.GetPlayerName();
        // ChangeImage();
    }
    void OnEnable()
    {
        isActive = true;
        Instruction();
    }

    void OnDisable()
    {
        isActive = false;
    }
    // public void ChangeImage ()
    // {  
    //     if (img != null) img = GetComponent<Image>();
    //     img.sprite = syntaxImage;

    // }
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
