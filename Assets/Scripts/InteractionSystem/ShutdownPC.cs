using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutdownPC : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Canvas _hud;
    [SerializeField] GameObject compCamera;
    [SerializeField] GameObject _mainCam;
    [SerializeField] GameObject computer;
    [SerializeField] GameObject loginAnimation;
    [SerializeField] GameObject[] screens;
    Computer _computer;

    public void _ShutDownPC()
    {
        //Play SoundFx
        AudioManager.Instance.PlaySfx("Shutdown");

        if (Panel != null)
        {
            // Instance
            _computer = computer.GetComponent<Computer>();
            bool isActive = Panel.activeSelf;

            // Hide the computer screen
            Panel.SetActive(!isActive);

            // Turn off all screens that are open 
            foreach (var screen in screens)
            {
                screen.SetActive(false);
            }

            _computer.isOpened = false;
            loginAnimation.SetActive(true);
            UIController.Instance.SetPanelActive(false);
            compCamera.gameObject.SetActive(false);
            _mainCam.SetActive(true);

            // Turn on main UI
            _hud.enabled = true;
        }
    }
}
