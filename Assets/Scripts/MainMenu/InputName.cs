using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputName : MonoBehaviour
{
    private string userName;
    private string fileName = "playerData.txt";
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject errorText;
    [SerializeField] private GameObject loading;
    LoadingScene _loadScene;

    private void Start() {
        _loadScene = loading.GetComponent<LoadingScene>();
    }

    public void SaveName(int sceneID) {
        userName = inputField.text;

        if(userName.Length < 4) {
            errorText.SetActive(true);
            return;
        }
        
        errorText.SetActive(false);
        transform.gameObject.SetActive(false);

        if(BinarySerializer.HasSaved(fileName)) {
            BinarySerializer.Delete(fileName);
        }

        DataManager.SetPlayerName(userName);
        _loadScene.LoadScene(sceneID);
    }

    public void ClosePanel() {
        transform.gameObject.SetActive(false);
    }
}
