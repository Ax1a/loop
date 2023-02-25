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

    //int level = 2;

    [SerializeField] GameObject[] lessonContents;

    void Start()
    {

        Transform parent = parentObject;
        Destroy(parent.GetChild(0).gameObject);
        btn();
        
    }

    void btn()
    {

        for (int i = 0; i < lessonIcon.Length; i++)
        { 
            GameObject button = Instantiate(buttonPrefab, container);
            //populates buttons
            int index = i;

            button.GetComponent<Image>().sprite = lessonIcon[index];

            button.GetComponentInChildren<TextMeshProUGUI>().text = lessonName[index];

            //adds different onclick for buttons
            button.GetComponent<Button>().onClick.AddListener(delegate {ToggleGameObject(lessonContents[index]);});  
            
            // button.GetComponentInChildren<Button>().interactable = false;

            // if(lessonIcon.Length >= level)
            // {

            // } 
        }

        // if(lessonIcon.Length >= level)
        // {
            
        //     for (int i = 0; i < level; i++)
        //     {
        //         // lessonBtn[i].interactable = true;
        //         button.GetComponentInChildren<Button>().interactable = true; 

        //     }

        // }


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
