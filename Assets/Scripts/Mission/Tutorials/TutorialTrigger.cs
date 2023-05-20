using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private string[] dialogues;
    [SerializeField] private string[] completeDialogues;
    [SerializeField] private string[] tasks;
    [SerializeField] private GameObject[] highlightGuides;
    [SerializeField] private string taskTitle;
    [SerializeField] private TextMeshProUGUI taskTitleTxt;
    [SerializeField] private TextMeshProUGUI taskPrefab;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private Transform taskParent;
    [SerializeField] private List<KeyCode> keyTasks;
    [SerializeField] private MazePlayerMovement mazeMovement;
    [SerializeField] private bool withKeyTask;
    private bool triggered;
    private bool tutorialComplete = false;

    private void Start() {
        mazeMovement.canJump = false;
        mazeMovement.canSprint = false;
        mazeMovement.canCrouch = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!triggered && ThirdPersonCamera.Instance.IsControlEnabled()) {
            if (keyTasks.Count > 0) {
                if (keyTasks[0] == KeyCode.Space) {
                    mazeMovement.canJump = true;
                }
                else if (keyTasks[0] == KeyCode.LeftControl) {
                    mazeMovement.canCrouch = true;
                }
                else if (keyTasks[0] == KeyCode.LeftShift) {
                    mazeMovement.canSprint = true;
                }
            }

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
        taskPanel.transform.DOScale(1.2f, 1).SetEase(Ease.OutBounce);

        TextMeshPro[] texts = taskPanel.GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro text in texts)
        {
            text.color = Color.green;
        }

        yield return new WaitForSeconds(.8f);

        foreach (TextMeshPro text in texts)
        {
            text.color = Color.white;
        }
        taskPanel.transform.DOScale(1f, 1).SetEase(Ease.InBounce);
        foreach (Transform child in taskParent)
        {
            Destroy(child.gameObject);
        }

        taskPanel.SetActive(false);
        if (tasks.Length > 0) AudioManager.Instance.PlaySfx("Correct");

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
