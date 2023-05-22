using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    public List<int> availableQuestIDs = new List<int>();
    public List<int> receivableQuestIDs = new List<int>();
    [SerializeField] private int unlockInteractionID = -1;
    public bool isAutoAccept = false;
    
    private void OnEnable() {
        QuestRequestObject();
    }

    public void QuestRequestObject() {
        if (unlockInteractionID != -1)
           if (InteractionQuizManager.Instance != null) InteractionQuizManager.Instance.ActivateInteractionQuiz(unlockInteractionID);

        QuestManager.Instance.QuestRequest(this);
        // QuestUI.Instance.DisplayFirstQuest();
    }

    // private void Update() {
        // if (Input.GetKeyDown(InputManager.Instance.interact) && interactObject.NearInteractable()) {
            // QuestManager.Instance.AddQuestItem("Finish lesson 1", 1);
        // }
    // }

}
