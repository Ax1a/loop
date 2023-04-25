using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionQuizManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> interactionQuizPanels;
    [SerializeField] private Transform c_quizBtnParent; // C++
    [SerializeField] private Transform p_quizBtnParent; // Python
    [SerializeField] private Transform j_quizBtnParent; // Java
    [SerializeField] private GameObject quizBtnPrefab;
    [SerializeField] private GameObject quizBtnPlaceholder;
    private List<InteractionQuiz> _interactionData = new List<InteractionQuiz>();
    public static InteractionQuizManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void OnEnable() {
        // Comment IF-Else for testing. Comment Else condition contents
        if (DataManager.GetInteractionQuizData().Count <= 0) {
            foreach (var panel in interactionQuizPanels)
            {
                InteractionQuiz info = panel.GetComponent<InteractionQuizInfo>().data;
                _interactionData.Add(info);
            }
            DataManager.AddInteractionQuizData(_interactionData);
        }
        else {
            _interactionData = DataManager.GetInteractionQuizData();

            // Set data for each panels
            for (int i = 0; i < interactionQuizPanels.Count; i++)
            {
                interactionQuizPanels[i].GetComponent<InteractionQuizInfo>().data = _interactionData[i];
            }
        }

        FillLanguageButtons();

        // Testing
        // foreach (var item in _interactionData)
        // {
        //     Debug.Log(item.quizID + ": " + item.isComplete);
        // }
    }

    public void EnablePanel(int id) {
        interactionQuizPanels[id].SetActive(true);
    }

    public void FillLanguageButtons() {
        // Initialize button counts for each language
        int c_btnCount = 0;
        int p_btnCount = 0;
        int j_btnCount = 0;

        // Clear existing buttons
        ClearButtons();

        // Create and sort buttons based on completeness
        List<GameObject> completeBtns = new List<GameObject>();
        List<GameObject> incompleteBtns = new List<GameObject>();

        for (int i = 0; i < interactionQuizPanels.Count; i++)
        {
            InteractionQuiz infoScript = _interactionData[i];
            
            if (infoScript.isActive) {
                GameObject quizBtn = null;

                // Instantiate button based on language
                if (infoScript.language == InteractionQuiz.quizLanguage.C) {
                    quizBtn = Instantiate(quizBtnPrefab, c_quizBtnParent);
                    c_btnCount++;
                }
                else if (infoScript.language == InteractionQuiz.quizLanguage.JAVA) {
                    quizBtn = Instantiate(quizBtnPrefab, j_quizBtnParent);
                    j_btnCount++;
                }
                else if (infoScript.language == InteractionQuiz.quizLanguage.PYTHON) {
                    quizBtn = Instantiate(quizBtnPrefab, p_quizBtnParent);
                    p_btnCount++;
                }

                if (quizBtn != null) {
                    InteractionQuizBtn btnScript = quizBtn.GetComponent<InteractionQuizBtn>();

                    btnScript.SetQuizTitle(infoScript.quizTitle);
                    btnScript.SetQuizDescription(infoScript.quizDescription);
                    btnScript.quizID = i;
                    infoScript.quizID = i;


                    if (infoScript.isComplete) {
                        btnScript.ShowCompleteIndicator();
                        completeBtns.Add(quizBtn);
                    }
                    else
                    {
                        incompleteBtns.Add(quizBtn);
                    }
                }
            }

        }    

        // Sort buttons based on completeness
        incompleteBtns.AddRange(completeBtns);
        for (int i = 0; i < incompleteBtns.Count; i++)
        {
            incompleteBtns[i].transform.SetSiblingIndex(i);
        }


        // Add placeholder image for each language
        for(int j = 0; j < 12 - c_btnCount; j++) {
            Instantiate(quizBtnPlaceholder, c_quizBtnParent);
        }

        for(int j = 0; j < 12 - p_btnCount; j++) {
            Instantiate(quizBtnPlaceholder, p_quizBtnParent);
        }
        
        for(int j = 0; j < 12 - j_btnCount; j++) {
            Instantiate(quizBtnPlaceholder, j_quizBtnParent);
        }
    }

    public void ClearButtons() {
        if (c_quizBtnParent.childCount > 0) {
            foreach (Transform item in c_quizBtnParent)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
        if (p_quizBtnParent.childCount > 0) {
            foreach (Transform item in p_quizBtnParent)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
        if (j_quizBtnParent.childCount > 0) {
            foreach (Transform item in j_quizBtnParent)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
    }

    /*
    
    Method Overloading
    for setting the quiz as complete or active

    Ex.
    InteractionQuizManager.Instance.ActivateInteractionQuiz();
     */

    /// <summary>
    /// Set the quiz interaction as complete based on the id
    /// </summary>
    /// <param name="id">id is the id of the quiz that has an attached InteractionQuizInfo</param>
    public void SetInteractionAsComplete(int id) {
        for (int i = 0; i < interactionQuizPanels.Count; i++)
        {
            InteractionQuiz info = _interactionData[i];

            if (info.quizID == id) {
                info.isComplete = true;
                DataManager.SetInteractionQuizComplete(i);
                FillLanguageButtons();
                SaveGame.Instance.SaveGameState();

                break;
            }
        }
    }

    /// <summary>
    /// Set the quiz interaction as complete based on the gameobject
    /// You can use this if you want to instantiate the script of gameobject instead of the id
    /// </summary>
    /// <param name="InteractionQuizInfo"> InteractionQuizInfo is the script that is attached on the InteractionQuiz Panel</param>
    public void SetInteractionAsComplete(InteractionQuizInfo panelInfo) {
        for (int i = 0; i < interactionQuizPanels.Count; i++)
        {
            InteractionQuiz info = _interactionData[i];

            if (info.quizID == panelInfo.data.quizID) {
                info.isComplete = true;
                DataManager.SetInteractionQuizComplete(i);
                FillLanguageButtons();
                SaveGame.Instance.SaveGameState();

                break;
            }
        }
    }
    
    /// <summary>
    /// Set the quiz interaction as active based on the gameobject
    /// You can use this if you want to instantiate the script of gameobject instead of the id
    /// </summary>
    /// <param name="InteractionQuizInfo"> InteractionQuizInfo is the script that is attached on the InteractionQuiz Panel</param>
    public void ActivateInteractionQuiz(InteractionQuizInfo panelInfo) {
        for (int i = 0; i < interactionQuizPanels.Count; i++)
        {
            InteractionQuiz info = _interactionData[i];

            if (info.quizID == panelInfo.data.quizID) {
                info.isActive = true;
                DataManager.ActivateInteractionQuiz(i);
                FillLanguageButtons();
                SaveGame.Instance.SaveGameState();

                break;
            }
        }
    }

    /// <summary>
    /// Set the quiz interaction as active based on the id
    /// </summary>
    /// <param name="id">id is the id of the quiz that has an attached InteractionQuizInfo</param>
    public void ActivateInteractionQuiz(int id) {
        for (int i = 0; i < interactionQuizPanels.Count; i++)
        {
            InteractionQuiz info = _interactionData[i];

            if (info.quizID == id) {
                info.isActive = true;
                DataManager.ActivateInteractionQuiz(i);
                FillLanguageButtons();
                SaveGame.Instance.SaveGameState();

                break;
            }
        }
    }

    // For presentation
    public void ActivateAllInteractionQuiz() {
        for (int i = 0; i < interactionQuizPanels.Count; i++)
        {
            InteractionQuiz info = _interactionData[i];
            info.isActive = true;
            DataManager.ActivateInteractionQuiz(i);
            FillLanguageButtons();
            SaveGame.Instance.SaveGameState();
        }
    }
}
