using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private GameObject mainMenu;

    [Header ("Programming Language Section")]
    [SerializeField] private GameObject errorTextProg;
    public Color selectedColor;
    public Color deselectedColor;
    [SerializeField] private Button[] progLanguageBtns;

    [Header ("Information Section")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject errorText;
    [SerializeField] private GameObject loading;
    [SerializeField] private TextMeshProUGUI selectedLanguageTxt;
    [SerializeField] private Image selectedLanguageImg;
    [SerializeField] private Sprite[] languageSprites;
    [SerializeField] private GameObject characterCamera;

    LoadingScene _loadScene;
    private string userName;
    private string fileName = "playerData.txt";
    private int selectedIndex = -1;
    
    private void Start() {
        _loadScene = loading.GetComponent<LoadingScene>();

        // Add a listener to each button to handle the click event
        for (int i = 0; i < progLanguageBtns.Length; i++)
        {
            int index = i;
            progLanguageBtns[i].onClick.AddListener(() => SelectButton(index));
        }
    }

    // Function for next panel
    private void TogglePanel(int panelIndex) {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }

        panels[panelIndex].SetActive(true);
    }


    // Button function for next 
    public void NextPanel(int panelIndex) {
        if (panelIndex - 1 == 0) {
            SaveCharacter();
            TogglePanel(panelIndex);
        }
        else if (panelIndex - 1 == 1) {
            if (selectedIndex == -1) {
                errorTextProg.SetActive(true);
            }
            else {
                SaveProgrammingLanguage();
                TogglePanel(panelIndex);
                errorTextProg.SetActive(false);
            }
        }
        else if (panelIndex - 1 == 2) {
            // save character name
            CreateNewGame();
        }
    }

    public void PreviousPanel(int panelIndex) {
        if (panelIndex != -1) {
            foreach (var panel in panels)
            {
                panel.SetActive(false);
            }

            panels[panelIndex].SetActive(true);
        }
        else {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
            characterCamera.SetActive(false);
        }

    }

    private void SaveCharacter() {
        // Add function to save character customize
        Debug.Log("Character Saved");
    }

    private void SaveProgrammingLanguage() {
        // Add function to save programming language
        Debug.Log("Programming Language Saved");

        selectedLanguageImg.sprite = languageSprites[selectedIndex];
        selectedLanguageImg.preserveAspect = true;

        if (selectedIndex == 0) {
            selectedLanguageTxt.SetText("C++");
        }
        else if (selectedIndex == 1) {
            selectedLanguageTxt.SetText("Python");
        }
        else if (selectedIndex == 2) {
            selectedLanguageTxt.SetText("Java");
        }
    }

    private void CreateNewGame() {
        Debug.Log("New Game");
        SaveName(1);
    }

    private async void SaveName(int sceneID) {
        userName = inputField.text;

        if(userName.Trim().Length < 4) {
            errorText.SetActive(true);
            return;
        }
        
        errorText.SetActive(false);
        transform.gameObject.SetActive(false);

        if(BinarySerializer.HasSaved(fileName)) {
            await Task.Run(() => BinarySerializer.Delete(fileName));
            DataManager.LoadPlayerData();
        }

        DataManager.SetPlayerName(userName);

        if (selectedIndex == 0) {
            DataManager.AddProgrammingLanguageProgress("c++");
        }
        else if (selectedIndex == 1) {
            DataManager.AddProgrammingLanguageProgress("python");
        }
        else if (selectedIndex == 2) {
           DataManager.AddProgrammingLanguageProgress("java");
        }

        _loadScene.LoadScene(sceneID);
    }

    // Function for choosing programming languages
    private void SelectButton(int index)
    {
        // Deselect the previously selected button
        if (selectedIndex >= 0)
        {
            progLanguageBtns[selectedIndex].GetComponent<Image>().color = deselectedColor;
        }

        // Select the new button
        selectedIndex = index;

        progLanguageBtns[selectedIndex].GetComponent<Image>().color = selectedColor;
    }
}
