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
    SaveGame _saveGame;

    [Header ("Menu Buttons")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button quitBtn;

    private void Start() {
        AddButtonEvents();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(gameUI[1].activeSelf == true || gameUI[2].activeSelf == true) return;

            ToggleUI("PauseMenu");
        }
        else if(Input.GetKeyDown(KeyCode.B)) {
            if (gameUI[4].activeSelf == true) return;

            ToggleUI("ShopUI");
        }
        else if(Input.GetKeyDown(KeyCode.I)) {
            if (gameUI[4].activeSelf == true) return;

            ToggleUI("InventoryCanvas");
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
