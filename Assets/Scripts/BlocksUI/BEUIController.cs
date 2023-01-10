using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BEUIController : MonoBehaviour
{
    //---Variables---

    BEController beController;

    [HideInInspector]
    /// <summary>
    /// Main Engine canvas.
    /// </summary>
    public Canvas mainCanvas;

    private int maxScreenSize;

    /// <summary>
    /// InputFiel for the BE Variable name 
    /// </summary>
    private InputField variableNameInput;

    /// <summary>
    /// Reference to the Variable block prefab, it is called "OperationGetVariable"
    /// </summary>
    public GameObject newVariable;

    /// <summary>
    /// Reference to the create variable UI panel
    /// </summary>
    public Transform newVariablePanel;

    [HideInInspector]
    public float uiScale;
    [HideInInspector]
    public float uiScaleDivisor;
    [HideInInspector]
    public bool enableCustomUIScale;

    public Transform BlocksPanel
    {
        get
        {
            if(mainCanvas == null)
            {
                mainCanvas = transform.GetComponentInChildren<Canvas>();
            }
            Transform panel = null;
            foreach (Transform child in mainCanvas.transform)
            {
                if (child.name == "Blocks Scroll View")
                {
                    panel = child.GetChild(0).GetChild(0);
                    break;
                }
            }
            return panel;
        }
    }

    // Icons expand/hide section
    public Sprite IconExpandDown;
    public Sprite IconExpandUp;

    //---Methods---

    void Start()
    {
        beController = GetComponent<BEController>();

        mainCanvas = transform.GetComponentInChildren<Canvas>();

        beController.ghostBlock = Instantiate(Resources.Load(beController.templatePrefabsPath + "GhostBlock", typeof(GameObject)), null) as GameObject;

        Rescale();
    }

    void Update()
    {
        int i = 0;
        foreach (BEVariable variable in beController.BeVariableList)
        {
            InputField inputField = newVariablePanel.parent.GetChild(beController.BeVariableList.Count - i + 2).GetComponentInChildren<InputField>();
            inputField.text = variable.value;
            i++;
        }
    }

    /// <summary>
    /// Toggles blocks section in BlocksPanel
    /// </summary>
    /// <param name="button"></param>
    public void ToggleSection(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        Transform sectionTransform = button.transform.parent;

        if (buttonImage.sprite.name == "Icon ExpandDown")
        {
            buttonImage.sprite = ExpandSection(sectionTransform);
        }
        else
        {
            buttonImage.sprite = CollapseSection(sectionTransform);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(BlocksPanel.GetComponent<RectTransform>());
    }

    Sprite ExpandSection(Transform sectionTransform)
    {
        for (int i = 1; i < sectionTransform.childCount; i++)
        {
            if (sectionTransform.GetChild(i) != newVariablePanel)
                sectionTransform.GetChild(i).gameObject.SetActive(true);
        }
        return IconExpandUp;
    }

    Sprite CollapseSection(Transform sectionTransform)
    {
        for (int i = 1; i < sectionTransform.childCount; i++)
        {
            sectionTransform.GetChild(i).gameObject.SetActive(false);
        }
        return IconExpandDown;
    }

    /// <summary>
    /// Expand or collapse all sections in BlocksPanel
    /// </summary>
    /// <param name="button"></param>
    public void ExpandCollapseSections(bool expand)
    {
        Transform blocksPanel = BlocksPanel;
        if (expand)
        {
            for (int i = 1; i < blocksPanel.childCount; i++)
            {
                Button button = blocksPanel.GetChild(i).GetComponentInChildren<Button>();
                Transform sectionTransform = button.transform.parent;
                Image buttonImage = button.GetComponent<Image>();
                buttonImage.sprite = ExpandSection(sectionTransform);
            }
        }
        else
        {
            for (int i = 1; i < blocksPanel.childCount; i++)
            {
                Button button = blocksPanel.GetChild(i).GetComponentInChildren<Button>();
                Transform sectionTransform = button.transform.parent;
                Image buttonImage = button.GetComponent<Image>();
                buttonImage.sprite = CollapseSection(sectionTransform);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(BlocksPanel.GetComponent<RectTransform>());
    }

    /// <summary>
    /// Open the create variable panel
    /// </summary>
    public void OpenCreateVariablePanel()
    {
        newVariablePanel.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(BlocksPanel.GetComponent<RectTransform>());
    }

    /// <summary>
    /// Create a new BE Variable based with name given in the variableNameInpu and set its value to 0
    /// </summary>
    public void CreateVariable()
    {
        variableNameInput = newVariablePanel.GetComponentInChildren<InputField>();

        string varName = variableNameInput.text.Replace(" ", "");
        if (varName != "")
        {
            InputField variableNameInput = newVariablePanel.GetComponentInChildren<InputField>();

            GameObject newVarWrapperPanel = Instantiate(Resources.Load(beController.templatePrefabsPath + "VariableWrapperPanel", typeof(GameObject)), newVariablePanel.parent) as GameObject;
            newVarWrapperPanel.transform.SetSiblingIndex(newVariablePanel.GetSiblingIndex() + 1);

            beController.SetVariable(variableNameInput.text, "0");
            GameObject newVar = Instantiate(newVariable, newVarWrapperPanel.transform);
            newVar.name = "OperationGetVariable";
            newVar.transform.GetChild(0).GetChild(0).GetComponent<InputField>().text = varName;
            newVar.transform.GetChild(0).GetChild(0).GetComponent<DynamicInputResize>().ExpandInputField();
            newVar.transform.SetSiblingIndex(1);
            CloseCreateVariablePanel();
        }
    }

    /// <summary>
    /// Close the create variable panel
    /// </summary>
    public void CloseCreateVariablePanel()
    {
        newVariablePanel.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(BlocksPanel.GetComponent<RectTransform>());
    }

    public void Rescale()
    {
        if (enableCustomUIScale)
        {
            if (uiScaleDivisor > 0)
            {
                uiScale = Screen.width / uiScaleDivisor;
                mainCanvas.scaleFactor = uiScale;
                beController.ghostBlock.GetComponent<RectTransform>().localScale = Vector3.one;
                foreach (BETargetObject target in BEController.beTargetObjectList)
                {
                    target.beProgrammingEnv.GetComponent<Canvas>().scaleFactor = uiScale;
                }
            }

            beController.ghostBlock.transform.localScale = Vector3.one * mainCanvas.scaleFactor;
        }
    }

}
