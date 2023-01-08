using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject popUp;
    private string FileName = "playerData.txt";
    private void Start() {
        // Display the continue button if there is saved data
        if (BinarySerializer.HasSaved(FileName)) {
            continueBtn.gameObject.SetActive(true);
        }
        else {
            continueBtn.gameObject.SetActive(false);
        }
    }

    public void NewGame() {
        // If no save data, just load to the main game
        if (!BinarySerializer.HasSaved(FileName)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        BinarySerializer.Delete(FileName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Cancel() {
        popUp.SetActive(false);
    }

    public void ContinueGame() {
        if (BinarySerializer.HasSaved(FileName)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
