using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject[] mainTabs;

    public void SelectMainTab(int tab) {
        for (int i = 0; i < mainTabs.Length; i++)
        {   
            mainTabs[i].SetActive(false);
        }

        mainTabs[tab - 1].SetActive(true);
    }
}
