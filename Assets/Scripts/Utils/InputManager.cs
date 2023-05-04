using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode shop;
    public KeyCode inventory;
    public KeyCode exit;
    public KeyCode interact;
    public KeyCode quest;
    public KeyCode openPhone;
    public KeyCode closePhone;
    public KeyCode skip;

    public static InputManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

}
