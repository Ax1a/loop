using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitDropdownSounds : MonoBehaviour
{
    Dropdown dropdown;

    void Awake()
    {
        BEBlock thisBlock = GetComponent<BEBlock>();
        thisBlock.InitializeBlock();

        dropdown = thisBlock.BlockHeader.GetChild(thisBlock.userInputIndexes[0]).GetComponent<Dropdown>();
        
        //populating dropdown
        dropdown.ClearOptions();
        foreach (AudioClip audio in thisBlock.BeController.beSoundsList)
        {
            dropdown.options.Add(new Dropdown.OptionData(audio.name));
        }
        dropdown.RefreshShownValue();
    }
}
