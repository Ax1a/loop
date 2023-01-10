using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject loading;
    LoadingScene _loadScene;

    private string FileName = "playerData.txt";
    private void Start() {
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
        // If no save data, just load to the main game
        if (!BinarySerializer.HasSaved(FileName)) {
            _loadScene.LoadScene(sceneID);
        }
        // else delete the data and load to the main game
        else {
            popUp.SetActive(true);
        }
    }

    void SetActiveGO() {
        popUp.gameObject.SetActive(true);
    }

    public void StartNewGame(int sceneID) {
        BinarySerializer.Delete(FileName);
        Debug.Log(DataManager.GetDay());
        _loadScene.LoadScene(sceneID);
    }

    public void Cancel() {
        popUp.SetActive(false);
    }

    public void ContinueGame(int sceneID) {
        if (BinarySerializer.HasSaved(FileName)) {
            _loadScene.LoadScene(sceneID);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
