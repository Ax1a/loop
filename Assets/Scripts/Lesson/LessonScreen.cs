using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LessonScreen : MonoBehaviour
{
    
    public GameObject buttonPrefab;
    public Sprite[] lessonIcon;
    public string[] lessonName;
    public Transform container;
    public Transform parentObject;

    [SerializeField] GameObject[] lessonContents;

    void Start()
    {

        Transform parent = parentObject;
        Destroy(parent.GetChild(0).gameObject);

        for (int i = 0; i < lessonIcon.Length; i++)
        { 
            //populates buttons
            int index = i;
            GameObject button = Instantiate(buttonPrefab, container);

            button.GetComponent<Image>().sprite = lessonIcon[index];
            button.GetComponentInChildren<TextMeshProUGUI>().text = lessonName[index];

            //adds different onclick for buttons
            button.GetComponent<Button>().onClick.AddListener(delegate {ToggleGameObject(lessonContents[index]);});  
        }
    }

    void ToggleGameObject(GameObject go) {
        Debug.Log(go);
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
