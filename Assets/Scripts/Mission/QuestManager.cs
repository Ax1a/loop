using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private int currentQuest = 0;
    [SerializeField] private GameObject[] quests;

    void Start()
    {
        currentQuest = DataManager.GetQuestProgress();
        
        quests[currentQuest].SetActive(true);

        // if (currentQuest <= 2) Tutorial.TutorialList(currentQuest);

    }
}
