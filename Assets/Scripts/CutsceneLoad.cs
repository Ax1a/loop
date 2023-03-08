using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoad : MonoBehaviour
{
    [SerializeField] private int sceneID;
    private void OnEnable() {
        SceneManager.LoadScene(sceneID);
        // AudioManager.Instance.PlayMusic("Day");

    }
}
