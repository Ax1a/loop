using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateScene : MonoBehaviour
{
    [SerializeField] private int sceneID;
    
    private void OnEnable() {
        SceneManager.UnloadSceneAsync(gameObject.scene);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneID));
    }
}
