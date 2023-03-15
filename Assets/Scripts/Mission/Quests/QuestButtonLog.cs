using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButtonLog : MonoBehaviour
{
    public int questID;
    public TextMeshProUGUI questTitle;

    public void ShowAllInfos() {
        QuestUI.Instance.ShowSelectedQuest(questID);
    }
}
