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
    [SerializeField] private GameObject languageIcon;
    [SerializeField] private TextMeshProUGUI[] progLanguagesTxt;
    [SerializeField] private Button[] buyBtns;
    [SerializeField] private GameObject[] lockedIndicator;
    [SerializeField] private GameObject[] purchasedIndicator;
    private string[] progLanguages = { "c++", "java", "python" }; // To be used
    private bool iconIsAdded = false;

    // To be updated // Only c++ supported for now
    // Start is called before the first frame update
    void Start()
    {
        DisplayLockIndicator();

        if (DataManager.GetProgrammingLanguageProgress("c++") >= 1) {
            iconIsAdded = true;
            languageIcon.SetActive(true);
        }
        else {
            languageIcon.SetActive(false);
        }

        // Add sale on the first language
        if (DataManager.FirstProgrammingLanguage() && DataManager.GetProgrammingLanguageProgress("c++") == 0) {
            progLanguagesTxt[0].text = "0";
        }

        // Add button listener to add buttons
        for (int i = 0; i < buyBtns.Length; i++)
        {
            int temp = i;
            buyBtns[i].onClick.RemoveAllListeners();
            buyBtns[i].onClick.AddListener(() => BuyProgrammingCourse(temp));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!iconIsAdded && DataManager.GetProgrammingLanguageProgress("c++") >= 1) {
            iconIsAdded = true;
            languageIcon.SetActive(true);
        }
    }

    private void BuyProgrammingCourse(int languageIndex) {
        if (languageIndex == 0) {
            DataManager.AddProgrammingLanguageProgress("c++");
            purchasedIndicator[0].SetActive(true);
            buyBtns[0].interactable = false;
            QuestManager.Instance.AddQuestItem("Buy any course from the shop", 1);
            AudioManager.Instance.PlaySfx("Purchase");
        }
        // To be updated // Make it loop through arrays
    }

    private void DisplayLockIndicator() {
        if (DataManager.GetProgrammingLanguageProgress("c++") == -1) {
            lockedIndicator[0].SetActive(true);
        }
        else if (DataManager.GetProgrammingLanguageProgress("c++") >= 1) {
            lockedIndicator[0].SetActive(false);
            purchasedIndicator[0].SetActive(true);
        }
        else {
            lockedIndicator[0].SetActive(false);
        }

        if (DataManager.GetProgrammingLanguageProgress("java") == -1) {
            lockedIndicator[1].SetActive(true);
        }
        else{
            lockedIndicator[1].SetActive(false);
        }

        if (DataManager.GetProgrammingLanguageProgress("python") == -1) {
            lockedIndicator[2].SetActive(true);
        }
        else{
            lockedIndicator[2].SetActive(false);
        }
    }
}