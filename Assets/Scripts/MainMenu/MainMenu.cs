using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject optionsPopUp;
    [SerializeField] private GameObject musicSettingPopUp;
    [SerializeField] private GameObject characterCreation;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject characterCamera;
    LoadingScene _loadScene;

    private string FileName = "playerData.txt";
    private void Start() {
        // AudioManager.Instance.PlayMusic("MainMenu");
        // Display the continue button if there is saved data
        if (BinarySerializer.HasSaved(FileName)) {
            continueBtn.gameObject.SetActive(true);
        }
        else {
            continueBtn.gameObject.SetActive(false);
        }

        _loadScene = loading.GetComponent<LoadingScene>();
    }

    public void NewGame(int sceneID) {
        AudioManager.Instance.PlaySfx("Button");
        // If no save data, just load to the main game
        if (!BinarySerializer.HasSaved(FileName)) {
            mainMenu.SetActive(false);
            characterCreation.SetActive(true);
        }
        // else delete the data and load to the main game
        else {
            popUp.SetActive(true);
        }
    }

    void SetActiveGO() {
        popUp.gameObject.SetActive(true);
    }

    public void StartNewGame() {
        AudioManager.Instance.PlaySfx("Button");
        popUp.SetActive(false);
        mainMenu.SetActive(false);
        characterCreation.SetActive(true);
        characterCamera.SetActive(true);
        
    }

    public void Options() {
        AudioManager.Instance.PlaySfx("Button");
        optionsPopUp.SetActive(true);
    }

    public void SoundOption(){
        AudioManager.Instance.PlaySfx("Button");
        optionsPopUp.SetActive(false);
        musicSettingPopUp.SetActive(true);
    }

    public void Cancel() {
        AudioManager.Instance.PlaySfx("Button");
        if (popUp.activeSelf)
        {
        popUp.SetActive(false);
        }

        if (optionsPopUp.activeSelf)
        {
        optionsPopUp.SetActive(false);
        }

        if (musicSettingPopUp.activeSelf)
        {
        musicSettingPopUp.SetActive(false);
        }
    }

    public void ContinueGame(int sceneID) {
        AudioManager.Instance.PlaySfx("Button");
        if (BinarySerializer.HasSaved(FileName)) {
            _loadScene.LoadScene(sceneID);
        }
        mainMenu.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
