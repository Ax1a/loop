using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAdder : MonoBehaviour
{
    [SerializeField] private List<Quest> questList = new List<Quest>();

    void Start()
    {
        QuestManager.Instance.questList.AddRange(questList);
    }
}
