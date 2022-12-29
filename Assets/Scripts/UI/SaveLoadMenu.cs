using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadMenu : SaveLoadCode
{
    public UIDrag target;
    public string TextInput
    {
        get
        {
            return transform.GetChild(2).GetComponent<InputField>().text;
        }
        set
        {
            transform.GetChild(2).GetComponent<InputField>().text = value;
        }
    }

    public Transform ProgrammingEnv
    {
        get
        {
            Transform env = null;
            foreach(Transform child in transform.parent)
            {
                if(child.name == "ProgrammingEnv")
                {
                    env = child;
                }
            }
            return env;
        }
    }

    public void UnselectToggles()
    {
        Transform filesContent = transform.GetChild(1).GetChild(0).GetChild(0);
        foreach (Transform child in filesContent)
        {
            child.GetComponent<Toggle>().isOn = false;
        }
    }

    void Start()
    {
        //inherited from SaveLoadCode
        blocksPrefabsPath = "prefabs/Blocks/";
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition))
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void GetSelectedBECodeName(string name)
    {
        TextInput = name;
    }

    public string FullPath
    {
        get
        {
            string fullPath = "";
            string fileName = Regex.Replace(TextInput, "[^\\w\\._]", "");
            if (TextInput.Length >= 3)
            {
                if (TextInput.Substring(TextInput.Length - 3, 3).ToLower() == ".be")
                {
                    fullPath = SavedCodesPath + fileName;
                }
                else
                {
                    fullPath = SavedCodesPath + fileName + ".BE";
                }
            }
            else
            {
                fullPath = SavedCodesPath + fileName + ".BE";
            }
            return fullPath;
        }
    }

    public void TryDelete()
    {
        if (TextInput.Length > 0)
        {
            bool confirmation = false;
            if (System.IO.File.Exists(FullPath))
            {
                transform.GetChild(3).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(false);
                transform.GetChild(5).gameObject.SetActive(false);
                transform.GetChild(6).gameObject.SetActive(false);
                transform.GetChild(8).gameObject.SetActive(true);
            }
            else
            {
                confirmation = true;
            }

            if (confirmation == true)
            {
                ConfirmDeleteCode();
            }
        }
    }

    public void CloseConfirmDeletePanel()
    {
        if(ProgrammingEnv.GetComponent<SaveLoadCode>().dialogOption == DialogOptions.save)
            transform.GetChild(3).gameObject.SetActive(true);
        if (ProgrammingEnv.GetComponent<SaveLoadCode>().dialogOption == DialogOptions.load)
            transform.GetChild(4).gameObject.SetActive(true);

        transform.GetChild(5).gameObject.SetActive(true);
        transform.GetChild(6).gameObject.SetActive(true);
        transform.GetChild(8).gameObject.SetActive(false);
    }

    public void ConfirmDeleteCode()
    {
        ProgrammingEnv.GetComponent<SaveLoadCode>().BEDeleteCode(FullPath);
        
        CloseConfirmDeletePanel();
    }

    public void TrySave()
    {
        if (TextInput.Length > 0)
        {
            bool confirmation = false;
            if (System.IO.File.Exists(FullPath))
            {
                transform.GetChild(3).gameObject.SetActive(false);
                transform.GetChild(5).gameObject.SetActive(false);
                transform.GetChild(6).gameObject.SetActive(false);
                transform.GetChild(7).gameObject.SetActive(true);
            }
            else
            {
                confirmation = true;
            }

            if (confirmation == true)
            {
                ConfirmSaveCode();
            }
        }
    }

    public void CloseConfirmSavePanel()
    {
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(5).gameObject.SetActive(true);
        transform.GetChild(6).gameObject.SetActive(true);
        transform.GetChild(7).gameObject.SetActive(false);
    }

    public void ConfirmSaveCode()
    {
        ProgrammingEnv.GetComponent<SaveLoadCode>().BESaveCode(FullPath);
        gameObject.SetActive(false);

        CloseConfirmSavePanel();
    }

    public void Load()
    {
        string fullPath;
        if (TextInput.Length > 0)
        {
            fullPath = SavedCodesPath + TextInput;

            ProgrammingEnv.GetComponent<SaveLoadCode>().BELoadCode(fullPath);
            gameObject.SetActive(false);
        }
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
