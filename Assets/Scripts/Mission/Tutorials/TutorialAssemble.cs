using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAssemble : Tutorial
{
    private bool isCurrentTutorial = false;
    [SerializeField] private GameObject Interactor;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject questGiver;
    public bool isComplete = false;
    InteractObject interactObject;

    private void Start() {
        interactObject = Interactor.GetComponent<InteractObject>();
    }

    public override void CheckIfHappening()
    {
        isCurrentTutorial = true;

        if (isComplete && isCurrentTutorial) {

            // Change the toggle box to check
            Transform _panel = parent.gameObject.transform.GetChild(0);
            QuestUISidePanel _questUI = _panel.GetComponent<QuestUISidePanel>();
            _questUI.ToggleCheck(true, 0);

            AudioManager.Instance.PlaySfx("Success");

            StartCoroutine(DelayAddQuest());
        }
    }

    private void ShowCompletedTutorial() {
        // Create new instance of the object class
        Quest _quest2 = new Quest();

        // Add values to the object
        _quest2.title = "Computer Assembly";
        _quest2.questType = Quest.QuestType.MAIN;

        // Call the banner popup class
        QuestUI.Instance.ShowCompleteQuestBanner(_quest2);

        
    }

    // For testing only // Fix add queue on popup
    private IEnumerator DelayAddQuest() {
        TutorialManager.Instance.CompletedTutorial();
        ShowCompletedTutorial();

        yield return new WaitForSeconds(.3f);

        Transform _parent = parent.transform;
        int objCount = _parent.childCount;

        if(objCount == 0) yield return null;

        foreach (Transform child in _parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        yield return new WaitForSeconds(2.5f);

        if (DataManager.GetProgrammingLanguageProgress("c++") >= 0) {
            QuestManager.Instance.EnableQuest(0);
        }
        else if (DataManager.GetProgrammingLanguageProgress("python") >= 0) {
            QuestManager.Instance.EnableQuest(16);
        }
        else if (DataManager.GetProgrammingLanguageProgress("java") >= 0) {
            QuestManager.Instance.EnableQuest(31);
        }
        
        questGiver.SetActive(true);

        SaveGame.Instance.SaveGameState();
    }    
}
