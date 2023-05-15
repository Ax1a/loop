using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private string[] dialogues;
    [SerializeField] private string[] completeDialogues;
    [SerializeField] private string[] tasks;
    [SerializeField] private string taskTitle;
    [SerializeField] private TextMeshProUGUI taskTitleTxt;
    [SerializeField] private TextMeshProUGUI taskPrefab;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private Transform taskParent;
    [SerializeField] private List<KeyCode> keyTasks;
    [SerializeField] private bool withKeyTask;
    private bool triggered;
    private bool tutorialComplete = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered) {
            taskPanel.SetActive(true);
            AddDialogue(dialogues);
            taskTitleTxt.gameObject.SetActive(true);
            taskTitleTxt.text = taskTitle;
            if (taskParent.childCount > 0) Destroy(taskParent.GetChild(0).gameObject);
            // Add tasks
            for (int i = 0; i < tasks.Length; i++)
            {
                TextMeshProUGUI _taskTxt = Instantiate(taskPrefab, taskParent);
                _taskTxt.text = tasks[i];
            }

            triggered = true;
        }
    }

    private void Update() {
        if (!withKeyTask) return;
        
        if (keyTasks.Count == 0 && !tutorialComplete && UIController.Instance.QueueIsEmpty()) {
            StartCoroutine(TutorialComplete());
        }
        else {
            if (!UIController.Instance.QueueIsEmpty() || !triggered) return;
            foreach (var key in keyTasks)
            {
                if (Input.GetKeyDown(key)) {
                    keyTasks.Remove(key);

                    break;
                }
            }
        }
    }

    public IEnumerator TutorialComplete() {
        tutorialComplete = true;
        yield return new WaitForSeconds(.5f);
        foreach (Transform child in taskParent)
        {
            Destroy(child.gameObject);
        }

        taskPanel.SetActive(false);

        AddDialogue(completeDialogues);
    }

    private void AddDialogue(string[] _dialogues) {
        // Add the dialogue
        foreach (var dialogue in _dialogues)
        {
            BotGuide.Instance.AddDialogue(dialogue);
        }
        BotGuide.Instance.ShowDialogue(); 
    }
}
