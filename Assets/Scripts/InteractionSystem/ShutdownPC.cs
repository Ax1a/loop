using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutdownPC : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject _hud;
    [SerializeField] GameObject compCamera;
    [SerializeField] GameObject computer;
    Computer _computer;

    public void _ShutDownPC()
    {   
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            _computer = computer.GetComponent<Computer>();

            Panel.SetActive(!isActive);

            _computer.isOpened = false;
            UIController.Instance.SetPanelActive(false);
            compCamera.gameObject.SetActive(false);
            _hud.gameObject.SetActive(true);
        }
    }
}
