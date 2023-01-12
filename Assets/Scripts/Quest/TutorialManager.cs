using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject[] TutorialsPanel;
    // [SerializeField] TextMeshProUGUI Title;
    // [SerializeField] TextMeshProUGUI SubTitle;
    // [SerializeField] TextMeshProUGUI Content;

    [Header("Animation")]
    [SerializeField] private float _cycleLength = 1;
    [SerializeField] private float _xCoord = 572f;
    [SerializeField] private float animationDelay = 1f;

    // [Header("Tutorial List")]
    // [SerializeField] private List<Quest> tutorialMissions;

    private int tutorialIndex;
    private int currentIndex;
    TextMeshProUGUI[][] tutorialContents;
    // private bool isDone;
    // private List<GameObject> _questPanels;
    void Start()
    {
        // _questPanels = new List<GameObject>();
        // // StartCoroutine(DelayAnimation(animationDelay));
        // foreach (var mission in tutorialMissions)
        // {
        //     GameObject panel = Instantiate(QuestPanel);
        //     
        //     panel.transform.SetParent(QuestPanel.transform.parent, false);
        //     panel.transform.localPosition = Vector3.zero;
            
        //     parent.Find("Title").GetComponentInChildren<TextMeshProUGUI>().text = mission.title;
        //     parent.Find("SubTitle").GetComponentInChildren<TextMeshProUGUI>().text = mission.subTitle;
            
        //     foreach (var contents in mission.content)
        //     {
        //         GameObject content = parent.Find("Content").gameObject;
        //         content = Instantiate(content);
        //         content.transform.SetParent(content.transform.parent, false);
        //         content.transform.localPosition = Vector3.zero;
        //         // parent.Find("Content").GetComponentInChildren<TextMeshProUGUI>().text = contents;
                
        //     }
        //     _questPanels.Add(panel);
        // }

        // // Destroy prefab
        // GameObject.Destroy(QuestPanel.gameObject);

        // _questPanels[tutorialIndex].SetActive(true);

        TutorialsPanel[0].SetActive(true);
        
        // var i = 0;
        // foreach (var panel in TutorialsPanel)
        // {
        //     Transform parent = panel.transform.Find("Container").transform.Find("Contents");
        //     TextMeshProUGUI[] contents = parent.GetComponentsInChildren<TextMeshProUGUI>();


        //     for (int j = 0; j < contents.Length; j++)
        //     {
        //         // tutorialContents[i][j] = contents[i];
        //         Debug.Log(contents[i].text);
        //     }

        //     i++;
        // }
    }

    private void Update() {
        if (tutorialIndex == 0) {
            if (Input.GetKeyDown(KeyCode.W)) {
                currentIndex++;
                // tutorialContents[0].color = new Color(52, 126, 69, 255);
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                currentIndex++;
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                currentIndex++;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                currentIndex++;
            }
            
            if (currentIndex == 4) {
                TutorialsPanel[0].SetActive(false);
                TutorialsPanel[1].SetActive(true);
                tutorialIndex++;
            }
        }
        else if(tutorialIndex == 1) {
            currentIndex = 0;
            
            if (Input.GetKeyDown(KeyCode.E)) {
                tutorialIndex++;
                TutorialsPanel[1].SetActive(false);
                TutorialsPanel[2].SetActive(true);
            }
        }
        else if (tutorialIndex == 2) {
            currentIndex = 0;
            if (Input.GetKeyDown(KeyCode.B)) {
                currentIndex++;
            }
            if (Input.GetKeyDown(KeyCode.I)) {
                currentIndex++;
                TutorialsPanel[2].SetActive(false);
            }
                
        }
    }

    // Bugged
    // IEnumerator DelayAnimation(float delayTime)
    // {

        // foreach (var quest in QuestPanel)
        // {
        //     quest.SetActive(true);
        //     quest.transform.DOMoveX(_xCoord, _cycleLength).SetEase(Ease.InOutSine);
        //     yield return new WaitForSeconds(delayTime);
        // }
    // }
}
