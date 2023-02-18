﻿//----------------------------------
// Version 1.0 (Sample)
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//The code implements a controllerSample for a sequence of functions using the theory of machine language and
// its way to arrange functions to be run in a organized sequence. It run thought a list and base on the function list.
// The objective is to induce the player to use a Drag and Drop Visual programming interface to write its own code to
// interact with the game or parts of the game.
// This is a sample of a bigger project being developed with loops/conditions, functions with variables and a simple
// textual programming language enable the translation between Visual (blocks) and Textual coding.
// 
//#Visual Block Coding:
// drag and drop from (https://github.com/danielcmcg/Unity-UI-Nested-Drag-and-Drop) functinos to the desired positions
// on the main loop of the object

public class controllerSample : MonoBehaviour
{
    public delegate void FuntionsList();
    // public GameObject mainTarget; //target object of the code is for

    public TextMeshProUGUI log;
    List<Function_> sequence; //list of functions (type Functions_). The code sequence is read from here
    private int isPlaying; 
    
    MainLoop loop1;

    public Transform varPanel;

    public GameObject newVar;

    private TMP_InputField varInput;

    public Transform newVarParent;
    
    public void createVar()
    {   
        varPanel.gameObject.SetActive(true);
    }
    public void go()
    {   
        varInput = varPanel.GetComponentInChildren<TMP_InputField>();

        string varName = varInput.text;
        
        Instantiate(newVar,newVarParent);

        newVar.name = "CloneVar";
        newVar.transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>().text = varName;
        Debug.Log(varName);
    }
    public void Paly()
    {
        sequence.Clear();
        sequence = TranslateCodeFromBlocks(transform.parent, sequence);
        
        loop1 = new MainLoop(log, sequence);
        StartCoroutine(loop1.Play());

        isPlaying = 2; 
    }

    public void Stop()
    {
        isPlaying = 1; 
    }

    void Start()
    {
        isPlaying = 0; 
        sequence = new List<Function_>();
    }
    
    void Update()
    {
        if (isPlaying == 2) //play
        {
            // loop1.infiniteLoop = transform.GetChild(1).GetComponent<Toggle>().isOn;
            // if (loop1.infiniteLoop && loop1.end)
            // {
                StartCoroutine(loop1.Play());
            // }
        }
        if (isPlaying == 1) //stop
        {
            StopCoroutine(loop1.Play());
        }
    }
    
    //recursive parser function
    private List<Function_> TranslateCodeFromBlocks(Transform parent, List<Function_> sequence_)
    {
        foreach (Transform child in parent)
        {
            var functionName = child.name.Split('_'); //looks like a little face ^^

            if (functionName[0] == "Function")
            {
                string function = functionName[1];
                switch (function)
                {
                    case "ChangeColor":
                        sequence_.Add(new ChangeColor_("ChangeColor"));
                        break;
                    case "MoveRight":
                        sequence_.Add(new MoveRight("MoveRight"));
                        break;
                    case "MoveLeft":
                        sequence_.Add(new MoveLeft("MoveLeft"));
                        break;
                    case "MoveUp":
                        sequence_.Add(new MoveUp("MoveUp"));
                        break;
                    case "MoveDown":
                        sequence_.Add(new MoveDown("MoveDown"));
                        break;
                    case "Jump":
                        sequence_.Add(new Jump("Jump"));
                        break;
                    case "If":
                        sequence_.Add(new If_("If"));
                        break;
                }
            }
        }
        
        return sequence_;
    }
    
}

public class MainLoop
{
    // GameObject mainTarget;

    TextMeshProUGUI log;
    List<Function_> sequence_;
    public bool infiniteLoop;
    public bool end;
    private float waitTime;

    public MainLoop(TextMeshProUGUI log, List<Function_> sequence_)
    {
        this.end = false;
        // this.mainTarget = mainTarget;
        this.log = log;
        this.sequence_ = sequence_;
        this.waitTime = 1.2f; //wait time between functions in sequence (list)
    }
    public IEnumerator Play()
    {
        WaitForSeconds wait = new WaitForSeconds(waitTime);
        this.end = false;
        foreach (Function_ fun in this.sequence_)
        {
            fun.Func(this.log);
            yield return wait;
        }
        this.end = true;
    }
    
}

public class Jump : Function_
{
    public Jump(string ID) : base(ID)
    {
        this.ID = ID;
    }

    override public void Func(TextMeshProUGUI log)
    {
        // mainTarget.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
        log.text = "Jump";
    }
}

public class MoveUp : Function_
{
    public MoveUp(string ID) : base(ID)
    {
        this.ID = ID;
    }

    override public void Func(TextMeshProUGUI log)
    {
        // mainTarget.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 300));
        Debug.Log("Moving up");
    }
}

public class MoveDown : Function_
{
    public MoveDown(string ID) : base(ID)
    {
        this.ID = ID;
    }

    override public void Func(TextMeshProUGUI log)
    {
        // mainTarget.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -300));
        log.text = "Moving Down";
        Debug.Log("Move Down");

    
    }
}

public class MoveRight : Function_
{
    public MoveRight(string ID) : base(ID)
    {
        this.ID = ID;
    }

    override public void Func(TextMeshProUGUI log)
    {
        // mainTarget.GetComponent<Rigidbody>().AddForce(new Vector3(300, 0, 0));
        log.text = "Move right";
    }
}

public class MoveLeft : Function_
{
    public MoveLeft(string ID) : base(ID)
    {
        this.ID = ID;
    }

    override public void Func(TextMeshProUGUI log)
    {
        // mainTarget.GetComponent<Rigidbody>().AddForce(new Vector3(-300, 0, 0));
        log.text = "Move Left";
        Debug.Log("Move left");
    }
}

public class ChangeColor_ : Function_
{
    public ChangeColor_(string ID) : base(ID)
    {
        this.ID = ID;
    }

    override public void Func(TextMeshProUGUI log)
    {
        // mainTarget.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        log.text = "Change Color";
    }
}

public class If_ : Function_
{
    public If_(string ID) : base(ID)
    {
        this.ID = ID;
    }

    override public void Func(TextMeshProUGUI log)
    {
        // mainTarget.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        log.text = "This is if Statement";
        Debug.Log("function ifff");
        
    }
}

public class Function_
{
    public string ID;

    //contructor for sinple functions
    public Function_(string ID)
    {
        this.ID = ID;
    }

    // public virtual void Func(GameObject mainTarget, TextMeshProUGUI log)
    // {

    // }
    public virtual void Func(TextMeshProUGUI log)
    {

    }

}