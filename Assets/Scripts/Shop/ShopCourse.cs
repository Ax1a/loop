using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCourse : MonoBehaviour
{
    /*  Index
        0 - C++
        1 - Java
        2 - Python
    */
    [SerializeField] private TextMeshProUGUI[] progLanguagePriceTxts;
    [SerializeField] private Button[] buyBtns;
    [SerializeField] private GameObject[] lockedIndicators;
    [SerializeField] private GameObject[] purchasedIndicators;
    [SerializeField] private GameObject[] courseSelectionLocks;
    [SerializeField] private GameObject[] courseSelectionProgress;
    [SerializeField] private GameObject insufficientMoneyTxt;
    [SerializeField] private TextMeshProUGUI reducedMoneyTxt;
    private string[] _progLanguages = { "c++", "java", "python" };
    private int[] _coursePrices = new int[3];
    private bool _iconIsAdded = false;
    private Coroutine _showCoroutineReduce, _showCoroutineInsufficient;

    void Start()
    {
        DisplayCourseStateIndicator();
        DisplayCoursePrices();

        // Add button listener to add buttons
        for (int i = 0; i < buyBtns.Length; i++)
        {
            int temp = i;
            buyBtns[i].onClick.RemoveAllListeners();
            buyBtns[i].onClick.AddListener(() => BuyProgrammingCourse(temp));
        }
    }

    private void BuyProgrammingCourse(int languageIndex) {
        if (_coursePrices[languageIndex] <= DataManager.GetMoney()) {
            if (_coursePrices[languageIndex] > 0) {
                if (_showCoroutineReduce != null) StopCoroutine(_showCoroutineReduce);
                _showCoroutineReduce = StartCoroutine(ShowReduceFunds(_coursePrices[languageIndex]));
            }

            DataManager.AddProgrammingLanguageProgress(_progLanguages[languageIndex]);
            purchasedIndicators[languageIndex].SetActive(true);
            buyBtns[languageIndex].interactable = false;

            DisplayCoursePrices();
            DisplayCourseStateIndicator();

            QuestManager.Instance.AddQuestItem("Buy any course from the shop", 1);
            AudioManager.Instance.PlaySfx("Purchase");
        }
        else {
            if (_showCoroutineInsufficient != null) StopCoroutine(_showCoroutineInsufficient);
            _showCoroutineInsufficient = StartCoroutine(ShowInsufficientText());
        }
    }

    private void DisplayCourseStateIndicator() {
        for (int i = 0; i < _progLanguages.Length; i++)
        {
            if (DataManager.GetProgrammingLanguageProgress(_progLanguages[i]) == -1) {
                lockedIndicators[i].SetActive(true);
                courseSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = false;
                courseSelectionLocks[i].SetActive(true);
                courseSelectionProgress[i].SetActive(false);
            }
            else if (DataManager.GetProgrammingLanguageProgress(_progLanguages[i]) >= 1) {
                lockedIndicators[i].SetActive(false);
                purchasedIndicators[i].SetActive(true);
                progLanguagePriceTxts[i].transform.parent.gameObject.SetActive(false);
                courseSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = true;
                buyBtns[i].gameObject.SetActive(false);

                courseSelectionProgress[i].SetActive(true);
                courseSelectionLocks[i].SetActive(false);
            }
            else {
                lockedIndicators[i].SetActive(false);
                courseSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = false;
                courseSelectionLocks[i].SetActive(true);
                courseSelectionProgress[i].SetActive(false);
            }
        }
    }

    private void DisplayCoursePrices() {
        int unlockedLanguages = DataManager.GetUnlockedProgrammingLanguageCount();

        for (int i = 0; i < _progLanguages.Length; i++)
        {
            // Add sale on the first language
            if (DataManager.FirstProgrammingLanguage() && DataManager.GetProgrammingLanguageProgress(_progLanguages[i]) == 0) {
                _coursePrices[i] = 0;
                progLanguagePriceTxts[i].text = _coursePrices[i].ToString();
            }
            else {
                _coursePrices[i] = unlockedLanguages != 0 ? unlockedLanguages * 1000 : 1000;
                progLanguagePriceTxts[i].text = _coursePrices[i].ToString();
            }
        }
    }

    IEnumerator ShowInsufficientText() {
        insufficientMoneyTxt.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        insufficientMoneyTxt.SetActive(false);
    }

    IEnumerator ShowReduceFunds(int price) {
        reducedMoneyTxt.gameObject.SetActive(true);
        reducedMoneyTxt.text = "-" + price.ToString();
        yield return new WaitForSeconds(1.5f);
        reducedMoneyTxt.gameObject.SetActive(false);
    }
}