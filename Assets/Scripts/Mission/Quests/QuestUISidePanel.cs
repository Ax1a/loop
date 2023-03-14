using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUISidePanel : MonoBehaviour
{
    [Header ("Information")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subTitle;
    [SerializeField] private GameObject requirement;
    [SerializeField] private Transform requirementParent;

    [Header ("Rewards")]
    [SerializeField] private GameObject rewards;
    [SerializeField] private TextMeshProUGUI amount;
    
    private List<GameObject> _objectives = new List<GameObject>();

    public void SetQuestTitle(string _title) {
        title.text = _title;
    }

    public void SetQuestSubTitle(string _title) {
        subTitle.text = _title;
    }

    public void SetQuestReward(string _reward) {
        amount.text = _reward;
    }

    public void GenerateObjectives(string[] objectives) {
        if (requirementParent.childCount > 0) Destroy(requirementParent.GetChild(0).gameObject);
        if (_objectives.Count > 0) _objectives.Clear();

        for (int i = 0; i < objectives.Length; i++)
        {
            _objectives.Add(Instantiate(requirement, requirementParent));
            _objectives[i].GetComponentInChildren<TextMeshProUGUI>().text = objectives[i];        
        }
    }

    public void ToggleCheck(bool check, int index) {
        _objectives[index].transform.GetChild(0).gameObject.SetActive(!check); // Unchecked box
        _objectives[index].transform.GetChild(1).gameObject.SetActive(check); // Checked box
    }
}
