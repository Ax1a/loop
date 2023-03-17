using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    private bool inTrigger = false;

    public List<int> availableQuestIDs = new List<int>();
    public List<int> receivableQuestIDs = new List<int>();
    [SerializeField] private GameObject Interactor;
    InteractObject interactObject;

    private void OnEnable() {
        interactObject = Interactor.GetComponent<InteractObject>();
        QuestManager.Instance.QuestRequest(this);
       
        QuestUI.Instance.DisplayFirstQuest();
        // Debug.Log
    }

    private void Update() {
        if (Input.GetKeyDown(InputManager.Instance.interact) && interactObject.NearInteractable()) {
            QuestManager.Instance.AddQuestItem("Interact", 1);
            QuestManager.Instance.AddQuestItem("Interact", 1);
            // QuestManager.Instance.AddQuestItem("Test", 1);
            Debug.Log("test");
        }
    }

}
