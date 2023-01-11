using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreen : MonoBehaviour
{
    public void openScreen(GameObject panel)
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
        }
    }
}
