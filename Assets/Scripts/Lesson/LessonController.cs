using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LessonController : MonoBehaviour
{
    [SerializeField] GameObject lessonBtnPrefab;
    [SerializeField] string[] lessonTitles;
    [SerializeField] GameObject[] lessonContents;

    void Start()
    {
        // Set the parent transform
        Transform parent = GameObject.Find("LessonList").transform;

        // Create a button for each string in the array
        for (int i = 0; i < lessonTitles.Length; i++)
        {
            int index = i;
            // Instantiate the button
            GameObject button = GameObject.Instantiate(lessonBtnPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            // Set the button's text
            button.GetComponentInChildren<TextMeshProUGUI>().text = lessonTitles[i];
            
            button.GetComponentInChildren<Button>().onClick.AddListener(delegate {ToggleGameObject(lessonContents[index]);});   
            
            // Set the button's parent
            button.transform.SetParent(parent);
        }

        GameObject.Destroy(parent.GetChild(1).gameObject);
    }

    public void ToggleGameObject(GameObject go)
    {
        if (go.activeInHierarchy == true)
        {
            return;
        }

        foreach (var item in lessonContents)
        {
            item.SetActive(false);
        }
        
        go.SetActive(true);
        
    }
}
