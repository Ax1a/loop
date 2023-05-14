using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    /* 
    * Add the GameUI here
    * Use ToggleUI function to open a UI

    * Use SetPanelActive(true) to stop the character movement
    */

    [Header ("Game Objects")]
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject indicatorCanvas;
    public GameObject onTopCanvas;
    [SerializeField] private GameObject[] gameUI;
    [SerializeField] private GameObject[] tabMenus;
    [SerializeField] private GameObject Character;
    [SerializeField] private GameObject[] highlightGuides;
    public Queue<GameObject> popUpUIs = new Queue<GameObject>();
    [SerializeField] private string sceneToLoad = "MainMenu";
    
    PlayerController _playerController;

    [Header ("Menu Buttons")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button quitBtn;

    private bool _anyActive;
    [HideInInspector] public bool gameUIActive;
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

        if (Character != null)
            _playerController = Character.GetComponent<PlayerController>();
    }

    void Update()
    {
        // Debug.Log("C++: " + DataManager.GetProgrammingLanguageProgress("c++"));

        if (!QueueIsEmpty()) {
            if (highlightGuides.Length > 0) {
                foreach (var highlightGuide in highlightGuides)
                {
                    if (highlightGuide.name == popUpUIs.Peek().name) {
                        highlightGuide.SetActive(true);
                    }   
                }
            }

            if (popUpUIs.Peek().name == "NewMissionPopUp" || popUpUIs.Peek().name == "MissionCompletePopUp"){
                popUpUIs.Peek().gameObject.SetActive(true);
            }

        }
        if (BotGuide.Instance.guideIsActive()) return;

        if(Input.GetKeyDown(InputManager.Instance.exit) && !gameUIActive){
            gameUIActive = true;
            mainUI.GetComponent<Canvas>().enabled = false;
            gameUI[1].SetActive(true);
            SetPanelActive(true);
        }
        else if(Input.GetKeyDown(InputManager.Instance.shop)) {
            gameUIActive = true;
            ToggleTab("Tab1", "ShopInventoryCanvas");
        }
        else if(Input.GetKeyDown(InputManager.Instance.inventory)) {
            gameUIActive = true;
            ToggleTab("Tab2", "ShopInventoryCanvas");
        }
        else if (Input.GetKeyDown(InputManager.Instance.quest)) {
            gameUIActive = true;
            ToggleTab("Container", "QuestCanvas");
        }
        
        if (_playerController == null) return;
        if (_anyActive == true) {
            _playerController.SetIsPanelActive(true);
        }
        else {
            _playerController.SetIsPanelActive(false);
        }
    }

    void AddButtonEvents() {
        // Prevent errors
        if (continueBtn != null) continueBtn.onClick.RemoveAllListeners();
        if (saveBtn != null) saveBtn.onClick.RemoveAllListeners();
        if (optionBtn != null) optionBtn.onClick.RemoveAllListeners();
        if (quitBtn != null) quitBtn.onClick.RemoveAllListeners();

        if (continueBtn != null) continueBtn?.onClick.AddListener(() => { CloseUI(1); });
        if (quitBtn != null) quitBtn?.onClick.AddListener(LoadNextScene);
        if (saveBtn != null) saveBtn?.onClick.AddListener(SaveGameData);
    }

    public void ToggleTab(string tabToOpen, string tabCanvas) {
        ToggleUI(tabCanvas);

        // Open the tab based on parameter
        foreach (var tab in tabMenus)
        {
            tab.SetActive(false);
            if (tab.name == tabToOpen) tab.SetActive(true);
        }
    }

    public void ToggleUI(string uiToOpen) {
        bool otherUIIsOpen = false;

        // Checking if other UI is open
        foreach (var ui in gameUI)
        {
            if (ui.activeSelf == true && (ui.name != uiToOpen && ui.name != "Guide")) {
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
                    indicatorCanvas.SetActive(false);
                    onTopCanvas.SetActive(false);
                    mainUI.GetComponent<Canvas>().enabled = false;

                    SetPanelActive(true);
                };
            }
        } 
    }

    void SaveGameData() {
        SaveGame.Instance.SaveGameState();
        CloseUI(1);
    }

    public void CloseUI(int index) {
        gameUI[index].SetActive(false);
        if (mainUI != null) mainUI.GetComponent<Canvas>().enabled = true;

        SetPanelActive(false);
        if (indicatorCanvas != null) indicatorCanvas.SetActive(true);
        if (onTopCanvas != null) onTopCanvas.SetActive(true);
        gameUIActive = false;
    }

    public void SetPanelActive(bool active) {
        _anyActive = active;
    }

    public bool OtherPanelActive() {
        return _anyActive;
    }

    public void DequeuePopupHighlight(int index) {
        for (int i = 0; i < highlightGuides.Length; i++)
        {
            if (QueueIsEmpty()) return;
            
            if (highlightGuides[i].name == popUpUIs.Peek().name) {
                popUpUIs.Peek().SetActive(false);
                Debug.Log(popUpUIs.Peek().name + ": Dequeued");
                popUpUIs.Dequeue();
            }
        }
    }

    public bool QueueIsEmpty() {
        return popUpUIs.Count == 0;
    }

    public void EnqueuePopup(GameObject popup) {
        popUpUIs.Enqueue(popup);
        Debug.Log(popup.name + ": Enqueued");
    }

    public void DequeuePopUp(GameObject popup) {
        if (popUpUIs.Peek().name == popup.name) {
            Debug.Log(popUpUIs.Peek().name + ": Dequeued");
            popUpUIs.Peek().SetActive(false);
            popUpUIs.Dequeue();
        }
    }

    void LoadNextScene() {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
