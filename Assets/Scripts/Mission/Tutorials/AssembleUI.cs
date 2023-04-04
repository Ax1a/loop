using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AssembleUI : MonoBehaviour
{
    [SerializeField] private Button buildBtn;
    [SerializeField] private GameObject TutorialObject;
    [SerializeField] private Button[] partBtns;
    [SerializeField] private Button[] itemBtns;
    [SerializeField] private GameObject[] partContainers;
    private List<Button> _selectedParts = new List<Button>();
    private bool _firstSelect = true;
    TutorialAssemble _tutorialAssemble;

    private void Update()
    {
        // Check if all of the parts have been picked
        if (_selectedParts.Count >= 7)
        {
            if (buildBtn.interactable) return;

            buildBtn.interactable = true;
            buildBtn.gameObject.GetComponent<Image>().color = Color.green;
        }
    }

    // On enable, add an onclick listener to the PC part buttons
    void OnEnable()
    {
        _firstSelect = true;

        for (int i = 0; i < partBtns.Length; i++)
        {
            int temp = i;
            partBtns[i].onClick.RemoveAllListeners();
            partBtns[i].onClick.AddListener(() => OpenPanel(temp));
        }
    }

    private void OnDisable() {
        if (UIController.Instance.popUpUIs.Count > 0)
        {
            if (_firstSelect || UIController.Instance.popUpUIs.Peek().name == "AssembleGuide") UIController.Instance.DequeuePopupHighlight(1);
        }
        _firstSelect = false;
    }

    // PC Part Type Onclick button
    void OpenPanel(int index)
    {
        if (partBtns.Length != partContainers.Length) return;

        for (int i = 0; i < partContainers.Length; i++)
        {
            partContainers[i].SetActive(false);

            if (i == index)
            {
                partContainers[i].SetActive(true);
            }
        }
    }

    // PC Part Item Onclick button
    public void SelectPCPart(int index)
    {
        if (UIController.Instance.popUpUIs.Count > 0)
        {
            if (_firstSelect || UIController.Instance.popUpUIs.Peek().name == "AssembleGuide") UIController.Instance.DequeuePopupHighlight(1);
        }
        _firstSelect = false;

        for (int i = 0; i < partContainers.Length; i++)
        {
            if (i == index)
            {
                partBtns[i].gameObject.GetComponent<Image>().color = Color.green;
                itemBtns[i].gameObject.GetComponent<Image>().color = new Color32(119, 238, 128, 255);
                if (!_selectedParts.Contains(itemBtns[i]))
                {
                    _selectedParts.Add(itemBtns[i]);
                }
            }
        }

        //Play SoundFx
        AudioManager.Instance.PlaySfx("Pc Parts Select");
    }

    public void CompletePCBuild()
    {
        _tutorialAssemble = TutorialObject.GetComponent<TutorialAssemble>();
        _tutorialAssemble.isComplete = true;
    }
}
