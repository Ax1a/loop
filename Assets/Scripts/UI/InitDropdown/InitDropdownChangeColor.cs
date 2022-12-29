using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitDropdownChangeColor : MonoBehaviour
{
    Dropdown dropdown;
    
    void Awake()
    {
        BEBlock thisBlock = GetComponent<BEBlock>();
        thisBlock.InitializeBlock();

        dropdown = thisBlock.BlockHeader.GetChild(thisBlock.userInputIndexes[0]).GetComponent<Dropdown>();
    
        //populating dropdown
        dropdown.ClearOptions();
        dropdown.options.Add(new Dropdown.OptionData("Random"));
        foreach (Material color in thisBlock.BeController.beColorsList)
        {
            dropdown.options.Add(new Dropdown.OptionData(color.name));
        }
        dropdown.RefreshShownValue();
    }
}
