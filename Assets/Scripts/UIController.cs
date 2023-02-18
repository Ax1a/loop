using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject dataManager;
    [SerializeField] private GameObject computerCanvas;
    [SerializeField] private GameObject[] gameUI;
    [SerializeField] private GameObject[] tabMenus;
    [SerializeField] private GameObject Character;
    PlayerController _playerController;
    SaveGame _saveGame;

    [Header ("Menu Buttons")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button quitBtn;
    private bool _anyActive;

    private void Start() {
        AddButtonEvents();
        _playerController = Character.GetComponent<PlayerController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(gameUI[1].activeSelf == true) return;

            ToggleUI("PauseMenu");
        }
        else if(Input.GetKeyDown(KeyCode.B)) {
            if (gameUI[3].activeSelf == true) return;

            gameUI[1].SetActive(true);
            tabMenus[0].SetActive(true);
            gameUI[0].SetActive(false);
        }
        // else if(Input.GetKeyDown(KeyCode.I)) {
        //     if (gameUI[4].activeSelf == true) return;

        //     ToggleUI("InventoryCanvas");
        // }

        foreach (var UI in gameUI)
        {
            if (UI.activeSelf == true && (UI.name != "Main UI" && UI.name != "InteractImage")) {
                _anyActive = true;
                break;
            }
            else {
                _anyActive = false;
            }
        }

        if (_anyActive == true) {
            _playerController.SetIsPanelActive(true);
        }
        else {
            _playerController.SetIsPanelActive(false);
        }
    }

    void AddButtonEvents() {
        // Prevent errors
        continueBtn.onClick.RemoveAllListeners();
        saveBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.RemoveAllListeners();

        continueBtn.onClick.AddListener(CloseMenu);
        quitBtn.onClick.AddListener(MainMenu);
        saveBtn.onClick.AddListener(SaveGameData);
    }

    void ToggleUI(string openUI) {
        if (computerCanvas.activeSelf == true) return;

        foreach (var ui in gameUI)
        {
            ui.SetActive(false);
        
            if(ui.name == openUI) ui.SetActive(true);
        }
    }

    void SaveGameData() {
        _saveGame = dataManager.GetComponent<SaveGame>();
        _saveGame.SaveGameState();
    }

    void CloseMenu() {
        gameUI[4].SetActive(false);
        gameUI[0].SetActive(true);
    }

    void MainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }
}
