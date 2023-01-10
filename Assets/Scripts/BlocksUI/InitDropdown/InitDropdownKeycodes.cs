using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitDropdownKeycodes : MonoBehaviour
{
    Dropdown dropdown;

    void Awake()
    {
        BEBlock thisBlock = GetComponent<BEBlock>();
        thisBlock.InitializeBlock();

        dropdown = thisBlock.BlockHeader.GetChild(thisBlock.userInputIndexes[0]).GetComponent<Dropdown>();
    
        //populating dropdown
        dropdown.ClearOptions();
        string[] keys = System.Enum.GetNames(typeof(KeyCode));
        foreach (string key in keys)
        {
            dropdown.options.Add(new Dropdown.OptionData(key));
        }
        dropdown.RefreshShownValue();
    }
}
