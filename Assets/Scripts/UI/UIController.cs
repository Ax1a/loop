using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject[] gameUI;
    [SerializeField] private GameObject[] tabMenus;
    [SerializeField] private GameObject Character;
    PlayerController _playerController;

    [Header ("Menu Buttons")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button quitBtn;

    [Header ("Keyboard Shortcuts")]
    [SerializeField] private KeyCode shop;
    [SerializeField] private KeyCode inventory;
    [SerializeField] private KeyCode menu;

    private bool _anyActive;
    public static UIController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start() {
        AddButtonEvents();
        _playerController = Character.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (BotGuide.Instance.guideIsActive()) return;

        if(Input.GetKeyDown(menu)){
            ToggleUI("PauseMenu");
        }
        else if(Input.GetKeyDown(shop)) {
            ToggleTab("Tab1");
        }
        else if(Input.GetKeyDown(inventory)) {
            ToggleTab("Tab2");
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

        continueBtn.onClick.AddListener(() => { CloseUI(2); });
        quitBtn.onClick.AddListener(GoToMainMenu);
        saveBtn.onClick.AddListener(SaveGameData);
    }

    void ToggleTab(string tabToOpen) {
        ToggleUI("TabMenuCanvas");

        // Open the tab based on parameter
        foreach (var tab in tabMenus)
        {
            tab.SetActive(false);
            if (tab.name == tabToOpen) tab.SetActive(true);
        }
    }

    void ToggleUI(string uiToOpen) {
        bool otherUIIsOpen = false;

        // Checking if other UI is open
        foreach (var ui in gameUI)
        {
            if (ui.activeSelf == true && (ui.name != uiToOpen && ui.name != "InteractImage" && ui.name != "Guide")) {
                Debug.Log(ui.name);
                otherUIIsOpen = true;
                break;
            }
        }

        // If there's no other UI open, open the UI
        if (otherUIIsOpen == false) {
            foreach (var ui in gameUI)
            {
                if (ui.name == uiToOpen) {
                    ui.SetActive(true);
                    gameUI[1].SetActive(false);
                    mainUI.SetActive(false);

                    SetPanelActive(true);
                };  
            }
        }
    }

    void SaveGameData() {
        SaveGame.Instance.SaveGameState();
    }

    public void CloseUI(int index) {
        gameUI[index].SetActive(false);
        mainUI.SetActive(true);

        SetPanelActive(false);
    }

    public void SetPanelActive(bool active) {
        _anyActive = active;
    }

    public bool otherPanelActive() {
        return _anyActive;
    }

    void GoToMainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }
}
