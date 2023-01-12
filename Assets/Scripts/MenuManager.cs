using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject interactionUI; 
    [SerializeField] private GameObject dataMngr;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject inventoryUI;
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
            if(shopUI.activeSelf == true) {
                shopUI.SetActive(false);
                return;
            }
            pauseUI.SetActive(!pauseUI.activeSelf);
            mainUI.SetActive(!mainUI.activeSelf);

            if(interactionUI.activeSelf == true) 
                interactionUI.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.B)) {
            shopUI.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.I)) {
            inventoryUI.SetActive(true);
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

    void SaveGameData() {
        _saveGame = dataMngr.GetComponent<SaveGame>();
        _saveGame.SaveGameState();
    }

    void CloseMenu() {
        pauseUI.SetActive(!pauseUI.activeSelf);
        mainUI.SetActive(!mainUI.activeSelf);
    }

    void MainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }
}
