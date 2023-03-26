using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButtonLog : MonoBehaviour
{
    public int questID;
    public TextMeshProUGUI questTitle;
    [SerializeField] private Sprite inactiveButtonSprite;
    [SerializeField] private Sprite activeButtonSprite;

    public void ShowAllInfos() {
        QuestUI.Instance.ShowSelectedQuest(questID);
        foreach (var button in QuestUI.Instance.questButtons)
        {
            SetInactiveSprite(button);

            if (button.GetComponent<QuestButtonLog>().questID == this.questID) {
                SetActiveSprite(button);
            }
        }
    }

    public void SetActiveSprite(GameObject button) {
        button.GetComponent<Image>().sprite = activeButtonSprite;
    }

    public void SetInactiveSprite(GameObject button) {
        button.GetComponent<Image>().sprite = inactiveButtonSprite;
    }
}
