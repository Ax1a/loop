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
    [SerializeField] private GameObject[] interactionQuizSelectionLocks;
    [SerializeField] private GameObject[] courseSelectionProgress;
    [SerializeField] private TextMeshProUGUI[] courseRequirementTxt;
    [SerializeField] private GameObject insufficientMoneyTxt;
    [SerializeField] private TextMeshProUGUI reducedMoneyTxt;
    private int courseLevelRequirement = 0;
    private string[] _progLanguages = { "c++", "java", "python" };
    private int[] _coursePrices = new int[3];
    private Coroutine _showCoroutineReduce, _showCoroutineInsufficient;
    public bool _checkedState = false;
    public static ShopCourse Instance;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (DataManager.GetTutorialProgress() >= 5 && !_checkedState) {
            DisplayCourseRequirements();
            DisplayCourseStateIndicator();
            UnlockNewCourse();

            // Add button listener to add buttons
            for (int i = 0; i < buyBtns.Length; i++)
            {
                int temp = i;
                buyBtns[i].onClick.RemoveAllListeners();
                buyBtns[i].onClick.AddListener(() => BuyProgrammingCourse(temp));
            }

            _checkedState = true;
        }
    }

    private void Update() {
        if (DataManager.GetTutorialProgress() >= 5 & !_checkedState) {
            DisplayCourseRequirements();
            DisplayCourseStateIndicator();

            // Add button listener to add buttons
            for (int i = 0; i < buyBtns.Length; i++)
            {
                int temp = i;
                buyBtns[i].onClick.RemoveAllListeners();
                buyBtns[i].onClick.AddListener(() => BuyProgrammingCourse(temp));
            }

            _checkedState = true;
        }
    }

    private void UnlockNewCourse() {
        bool existingCourseUnlocked = false;

        for (int i = 0; i < _progLanguages.Length; i++)
        {
            if (DataManager.GetProgrammingLanguageProgress(_progLanguages[i]) >= 10) {
                for (int j = 0; j < _progLanguages.Length; j++)
                {
                    if ((DataManager.GetProgrammingLanguageProgress(_progLanguages[j]) >= 1) && (DataManager.GetProgrammingLanguageProgress(_progLanguages[j]) <= 9)) {
                        existingCourseUnlocked = true;
                        break;
                    }
                }

                if (!existingCourseUnlocked) {
                    foreach (var item in _progLanguages)
                    {
                        if (item.ToLower() == "c++" && item != _progLanguages[i]) {
                            if (DataManager.GetProgrammingLanguageProgress(item) == -1) {
                                DataManager.SetProgrammingLanguageProgress(item, 0);
                            }
                        }
                        else if (item.ToLower() == "java" && item != _progLanguages[i]) {
                            if (DataManager.GetProgrammingLanguageProgress(item) == -1) {
                                DataManager.SetProgrammingLanguageProgress(item, 0);
                            }

                        }
                        else if (item.ToLower() == "python" && item != _progLanguages[i]) {
                            if (DataManager.GetProgrammingLanguageProgress(item) == -1) {
                                DataManager.SetProgrammingLanguageProgress(item, 0);
                            }
                        }
                    }
                }
            }
        }
    }

    private void BuyProgrammingCourse(int languageIndex) {
        if (_coursePrices[languageIndex] <= DataManager.GetMoney()) {
            QuestManager.Instance.AddQuestItem("Buy any course from the shop", 1);
            AudioManager.Instance.PlaySfx("Purchase");

            DataManager.SpendMoney(_coursePrices[languageIndex]);

            if (_coursePrices[languageIndex] > 0) {
                if (_showCoroutineReduce != null) StopCoroutine(_showCoroutineReduce);
                _showCoroutineReduce = StartCoroutine(ShowReduceFunds(_coursePrices[languageIndex]));
            }
            
            if (languageIndex == 0) {
                QuestManager.Instance.EnableQuest(0);
            }
            else if (languageIndex == 1) {
                QuestManager.Instance.EnableQuest(31);
            }
            else if (languageIndex == 2) {
                QuestManager.Instance.EnableQuest(16);
            }

            DataManager.AddProgrammingLanguageProgress(_progLanguages[languageIndex]);
            purchasedIndicators[languageIndex].SetActive(true);
            buyBtns[languageIndex].interactable = false;

            foreach (var course in _progLanguages)
            {
                if (DataManager.GetProgrammingLanguageProgress(course) == 0) {
                    DataManager.SetProgrammingLanguageProgress(course, -1);
                }
            }

            DisplayCourseRequirements();
            DisplayCourseStateIndicator();
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
                if (courseLevelRequirement != 0) {
                    courseRequirementTxt[i].gameObject.SetActive(true);
                    courseRequirementTxt[i].text = "Finish current course \nRequires Level " + courseLevelRequirement;
                }
                else {
                    courseRequirementTxt[i].gameObject.SetActive(false);
                }
                lockedIndicators[i].SetActive(true);
                courseSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = false;
                courseSelectionLocks[i].SetActive(true);
                interactionQuizSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = false;
                interactionQuizSelectionLocks[i].SetActive(true);
                courseSelectionProgress[i].SetActive(false);
            }
            else if (DataManager.GetProgrammingLanguageProgress(_progLanguages[i]) >= 1) {
                lockedIndicators[i].SetActive(false);
                courseRequirementTxt[i].gameObject.SetActive(false);
                purchasedIndicators[i].SetActive(true);
                progLanguagePriceTxts[i].transform.parent.gameObject.SetActive(false);
                courseSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = true;
                interactionQuizSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = true;
                interactionQuizSelectionLocks[i].SetActive(false);
                buyBtns[i].gameObject.SetActive(false);

                courseSelectionProgress[i].SetActive(true);
                courseSelectionLocks[i].SetActive(false);
            }
            else {
                if (DataManager.GetPlayerLevel() >= courseLevelRequirement) {
                    lockedIndicators[i].SetActive(false);
                    courseRequirementTxt[i].gameObject.SetActive(false);
                }
                else {
                    courseRequirementTxt[i].gameObject.SetActive(true);
                    courseRequirementTxt[i].text = "Finish current course \nRequires Level " + courseLevelRequirement;
                    lockedIndicators[i].SetActive(true);
                }
                courseSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = false;
                courseSelectionLocks[i].SetActive(true);
                interactionQuizSelectionLocks[i].transform.parent.GetComponent<Button>().interactable = false;
                interactionQuizSelectionLocks[i].SetActive(true);
                courseSelectionProgress[i].SetActive(false);
            }
        }
    }

    private void DisplayCourseRequirements() {
        courseLevelRequirement = 0;
        int unlockedLanguages = DataManager.GetUnlockedProgrammingLanguageCount();

        for (int i = 0; i < _progLanguages.Length; i++)
        {
            // Add sale on the a language
            if (DataManager.FirstProgrammingLanguage() && DataManager.GetProgrammingLanguageProgress(_progLanguages[i]) == 0) {
                _coursePrices[i] = 0;
                progLanguagePriceTxts[i].text = _coursePrices[i].ToString();
                courseLevelRequirement = 0;
            }
            else {
                _coursePrices[i] = unlockedLanguages != 0 ? unlockedLanguages * 1000 : 1000;
                courseLevelRequirement = 10 * unlockedLanguages;
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