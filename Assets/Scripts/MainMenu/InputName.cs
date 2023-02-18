using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

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

    public async void SaveName(int sceneID) {
        userName = inputField.text;

        if(userName.Trim().Length < 4) {
            errorText.SetActive(true);
            return;
        }
        
        errorText.SetActive(false);
        transform.gameObject.SetActive(false);

        if(BinarySerializer.HasSaved(fileName)) {
            await Task.Run(() => BinarySerializer.Delete(fileName));
        }

        DataManager.SetPlayerName(userName);
        _loadScene.LoadScene(sceneID);
    }

    public void ClosePanel() {
        transform.gameObject.SetActive(false);
    }
}
