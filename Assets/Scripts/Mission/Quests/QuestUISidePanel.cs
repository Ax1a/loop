using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuestUISidePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header ("Information")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subTitle;
    [SerializeField] private GameObject requirement;
    [SerializeField] private Transform requirementParent;

    [Header ("Background")]
    [SerializeField] private Image questBg;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float targetAlpha;

    [Header ("Rewards")]
    [SerializeField] private GameObject rewards;
    [SerializeField] private TextMeshProUGUI amount;
    
    private List<GameObject> _objectives = new List<GameObject>();

    private void OnEnable() {
        if (LayoutRefresher.Instance != null)
            LayoutRefresher.Instance.RefreshContentFitter((RectTransform)gameObject.transform.parent);
    }

    public void OnPointerEnter(PointerEventData pointerData) {
        StartCoroutine(FadeImage(targetAlpha));
    }

    public void OnPointerExit(PointerEventData pointerData) {
        StartCoroutine(FadeImage(40));
    }

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

    private IEnumerator FadeImage(float _targetAlpha)
    {
        Color originalColor = questBg.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, _targetAlpha / 255f);
        
        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / fadeDuration;
            questBg.color = Color.Lerp(originalColor, targetColor, normalizedTime);
            yield return null;
        }

        questBg.color = targetColor; // Ensure final alpha value is set correctly
    }
}
