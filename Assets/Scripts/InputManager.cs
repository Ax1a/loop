using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode shop;
    public KeyCode inventory;
    public KeyCode menu;
    public KeyCode interact;

    public static InputManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

}
