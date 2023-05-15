using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAdderNPC : MonoBehaviour
{
    [SerializeField] private List<Quest> questList = new List<Quest>();
    [SerializeField] private GameObject receiveQuestIndicator;
    [SerializeField] private GameObject completeQuestIndicator;

    void Start()
    {
        QuestManager.Instance.questList.AddRange(questList);
    }
}
