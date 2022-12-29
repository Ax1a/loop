using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BEBlock : MonoBehaviour
{
    public enum BlockTypeItems { trigger, simple, loop, conditional, operation };
    /// <summary>
    /// Type of the block. Used as substitute of GameObject.tag to guarantee the corret type after instantiate
    /// trigger: starter blocks for a sequence (sprite: 'Trigger Block'). Ex.: when play clicked, when button pressed
    /// simple: blocks that execute a single instruction per cycle and doesn't wrap children blocks (sprite: 'Simple Block'). Ex.: move x steps, turn x degrees
    /// loop: execute loop instruction wrapping children blocks (sprite: 'Loop Block'). Ex.: repeat x times, repeat forever
    /// conditional: execute conditional instruction and wrapp children blocks that will be executed if the condition is met (sprite: 'Loop Block'). Ex.: if [condition] then
    /// operation: input blocks that execute operations and return the result (string) as input for other blocks (sprite: 'Operation Block'). Ex.: x+y, x=y, variable
    /// <summary>
    public BlockTypeItems blockType;

    private BEInputs beInputs;
    /// <summary>
    /// Input values from the block header.
    /// See class BEInputs sumary
    /// </summary>
    public BEInputs BeInputs { get => beInputs; }

    private Transform blockHeader;
    /// <summary>
    /// Reference for the required block header.
    /// The block header containis a descriptive name for the block and and input spots, if needed and is the 0 index child of any block.
    /// </summary>
    public Transform BlockHeader { get => blockHeader; }

    //private Transform bottomDrop;
    //public Transform BottomDrop { get { return bottomDrop; } }

    public Outline blockOutline;

    public List<int> userInputIndexes = new List<int>();

    /// <summary>
    /// List of blocks that are wrapped inside this block.
    /// This list is used by Trigger, Loop and Conditional blocks.
    /// </summary>
    public List<BEBlock> beChildBlocksList;

    private BEBlock beBlockGroup;
    /// <summary>
    /// Block group that this block is part of. Used by the controller as reference of a single sequence of blocks.
    /// Setting this value also transmit the same value to the child blocks.
    /// </summary>
    public BEBlock BeBlockGroup
    {
        get
        {
            return beBlockGroup;
        }
        set
        {
            beBlockGroup = value;
            foreach (BEBlock childBlock in beChildBlocksList)
            {
                childBlock.BeBlockGroup = value;
            }
        }
    }

    /// <summary>
    /// Value set only for Trigger blocks to control which block (instruction) needs to be called in a respective sequence.
    /// </summary>
    public BEBlock beActiveBlock;

    /// <summary>
    /// Defines a block as active. If true, call the respective instruction.
    /// </summary>
    public bool isActive = false;

    /// <summary>
    /// Variable used by blocks that need to be executed during multiple cycles before go to next, as wait or loop instructions.
    /// </summary>
    public bool beBlockFirstPlay;

    /// <summary>
    /// Variable used by loop instructions so each block has its own counter.
    /// </summary>
    public float beBlockCounter;

    /// <summary>
    /// BE Controller reference
    /// </summary>
    private static BEController beController;
    public BEController BeController { get => beController; }

    /// <summary>
    /// Reference to the Target Object that the instruction aims to
    /// </summary>
    public BETargetObject beTargetObject;

    /// <summary>
    /// Path to the prefab blocks folder inside resources
    /// </summary>
    string blockPrefabsPath;

    string spritesPath;

    //string uiResourcesPath = "prefabs/UI/";

    /// <summary>
    /// Reference for the block Rect Transform
    /// </summary>
    public RectTransform rectTransform;

    /// <summary>
    /// Setup the basic layout values for each type of block used inside OnValidate
    /// This method is used as a helper when creating or adjusting the blocks, but it is not needed for running the Engine or builds
    /// </summary>
    public void SetBlocksLayout()
    {
        spritesPath = "Sprites/";
        if (!Application.isPlaying)
        {
            ContentSizeFitter contentSizeFitter;
            if (GetComponent<ContentSizeFitter>())
            {
                contentSizeFitter = GetComponent<ContentSizeFitter>();
            }
            else
            {
                contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            }

            VerticalLayoutGroup verticalLayoutGroup;
            if (GetComponent<VerticalLayoutGroup>())
            {
                verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            }
            else
            {
                verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            }

            if (GetComponent<UIDrag>() == null)
            {
                gameObject.AddComponent<UIDrag>();
            }

            Transform blockHeader = transform.GetChild(0);
            Image blockImage = GetComponent<Image>();

            if (blockType == BlockTypeItems.trigger)
            {
                blockHeader.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40);
                blockHeader.GetComponent<HorizontalLayoutGroup>().padding.left = 20;
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                verticalLayoutGroup.padding = new RectOffset(0, 5, 15, 20);
                verticalLayoutGroup.spacing = -5;
                verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
                verticalLayoutGroup.childControlWidth = false;
                verticalLayoutGroup.childControlHeight = false;
                verticalLayoutGroup.childForceExpandWidth = true;
                verticalLayoutGroup.childForceExpandHeight = true;
                if (blockImage.sprite.name != "Trigger Block")
                {
                    Sprite sprite = Resources.Load<Sprite>(spritesPath + "Trigger Block");
                    blockImage.sprite = sprite;
                }
            }

            if (blockType == BlockTypeItems.simple)
            {
                blockHeader.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 35);
                blockHeader.GetComponent<HorizontalLayoutGroup>().padding.left = 0;
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                verticalLayoutGroup.padding = new RectOffset(20, 5, 5, 5);
                verticalLayoutGroup.spacing = 0;
                verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
                verticalLayoutGroup.childControlWidth = false;
                verticalLayoutGroup.childControlHeight = false;
                verticalLayoutGroup.childForceExpandWidth = true;
                verticalLayoutGroup.childForceExpandHeight = true;
                if (blockImage.sprite.name != "Simple Block")
                {
                    Sprite sprite = Resources.Load<Sprite>(spritesPath + "Simple Block");
                    blockImage.sprite = sprite;
                }
            }

            if (blockType == BlockTypeItems.loop || blockType == BlockTypeItems.conditional)
            {
                blockHeader.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40);
                blockHeader.GetComponent<HorizontalLayoutGroup>().padding.left = 0;
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                verticalLayoutGroup.padding = new RectOffset(20, 5, 5, 30);
                verticalLayoutGroup.spacing = -5;
                verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
                verticalLayoutGroup.childControlWidth = false;
                verticalLayoutGroup.childControlHeight = false;
                verticalLayoutGroup.childForceExpandWidth = true;
                verticalLayoutGroup.childForceExpandHeight = true;
                if (blockImage.sprite.name != "Loop Block")
                {
                    Sprite sprite = Resources.Load<Sprite>(spritesPath + "Loop Block");
                    blockImage.sprite = sprite;
                }
            }

            if (blockType == BlockTypeItems.operation)
            {
                // v1.3 -Autoset and fix Operation Blocks' inputs size and alignment
                blockHeader.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 30);
                HorizontalLayoutGroup horizontalLayoutGroup = blockHeader.GetComponent<HorizontalLayoutGroup>();
                horizontalLayoutGroup.padding.left = 0;
                horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                GetComponent<RectTransform>().sizeDelta = new Vector2(100, 30);
                verticalLayoutGroup.padding = new RectOffset(20, 5, 0, 5);
                verticalLayoutGroup.spacing = 5;
                verticalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
                verticalLayoutGroup.childControlWidth = false;
                verticalLayoutGroup.childControlHeight = false;
                verticalLayoutGroup.childForceExpandWidth = true;
                verticalLayoutGroup.childForceExpandHeight = true;
                if (blockImage.sprite.name != "Operation Block")
                {
                    Sprite sprite = Resources.Load<Sprite>(spritesPath + "Operation Block");
                    blockImage.sprite = sprite;
                }

                foreach (Transform child in blockHeader.transform)
                {
                    if (child.GetComponent<InputField>())
                    {
                        InputField inputField = child.GetComponent<InputField>();
                        inputField.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 25);

                        // v1.3 -Autoset and fix Operation Blocks' inputs size and alignment
                        DynamicInputResize dynamicInputResize = inputField.GetComponent<DynamicInputResize>();
                        dynamicInputResize.RectTransform = inputField.GetComponent<RectTransform>();
                        dynamicInputResize.ResizeInputField();

                        RectTransform rectTransformPlaceholder = inputField.placeholder.rectTransform;
                        rectTransformPlaceholder.offsetMax = new Vector2(rectTransformPlaceholder.offsetMax.x, -5);
                        rectTransformPlaceholder.offsetMin = new Vector2(rectTransformPlaceholder.offsetMin.x, 4);

                        RectTransform rectTransformText = inputField.textComponent.rectTransform;
                        rectTransformText.offsetMax = new Vector2(rectTransformText.offsetMax.x, -5);
                        rectTransformText.offsetMin = new Vector2(rectTransformText.offsetMin.x, 4);
                    }
                    if (child.GetComponent<Dropdown>())
                    {
                        Dropdown dropdown = child.GetComponent<Dropdown>();
                        dropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 25);

                        DynamicInputResize dynamicInputResize = dropdown.GetComponent<DynamicInputResize>();
                        dynamicInputResize.RectTransform = dropdown.GetComponent<RectTransform>();
                        dynamicInputResize.ResizeInputField();

                        RectTransform rectTransformLabel = dropdown.captionText.rectTransform;
                        rectTransformLabel.offsetMax = new Vector2(rectTransformLabel.offsetMax.x, -5);
                        rectTransformLabel.offsetMin = new Vector2(rectTransformLabel.offsetMin.x, 4);
                    }
                }
            }

            if (blockType == BlockTypeItems.trigger || blockType == BlockTypeItems.loop || blockType == BlockTypeItems.conditional)
            {
                if (GetComponent<UIDrop>() == null)
                {
                    gameObject.AddComponent<UIDrop>();
                }
            }
            // v1.2 -Destroy UIDrop if dont fit on the Block type to avoid potential error 
            if (blockType != BlockTypeItems.trigger && blockType == BlockTypeItems.loop && blockType == BlockTypeItems.conditional)
            {
                if (GetComponent<UIDrop>() != null)
                {
                    DestroyImmediate(gameObject.GetComponent<UIDrop>());
                }
            }
        }
    }

    private void OnValidate()
    {
        SetBlocksLayout();
    }

    private void Update()
    {
        if (beBlockGroup != null)
        {
            blockOutline.enabled = beBlockGroup.isActive;
        }
        else
        {
            blockOutline.enabled = false;
        }
    }

    void Awake()
    {
        beBlockFirstPlay = true;
        InitializeBlock();

        blockPrefabsPath = "prefabs/Blocks/";
        spritesPath = "Sprites/";

        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }

    /// <summary>
    /// Error check for initializing the inputs of the block header to the beInputs variable
    /// </summary>
    public void InitializeInputs()
    {
        try
        {
            beInputs = GetAllUserInputs();
        }
        catch (Exception e)
        {
            Debug.Log("probably the block has no inputs");
            Debug.Log(e);
        }
    }

    // map the variables of the block
    /// <summary>
    /// Initializes the main needed variables of the block and maps the inputs of the block header
    /// </summary>
    public void InitializeBlock()
    {
        blockOutline = GetComponent<Outline>();
        blockHeader = transform.GetChild(0);
        beController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BEController>();

        // Maps the inputs of the block header
        userInputIndexes.Clear();
        for (int i = 0; i < BlockHeader.childCount; i++)
        {
            if (BlockHeader.GetChild(i).GetComponent<InputField>())
            {
                userInputIndexes.Add(i);
            }
            else if (BlockHeader.GetChild(i).GetComponent<Dropdown>())
            {
                userInputIndexes.Add(i);
            }
        }
    }

    /// <summary>
    /// Get the value in string of the user input by index.
    /// Dynamic means that the input can be of types InputField, Dropdown or an Operation block.
    /// </summary>
    /// <param name="index">index of the userinput</param>
    /// <returns>(string)value of the dynamic user input</returns>
    public string GetDynamicUserInput(int index)
    {
        if (BlockHeader.GetChild(userInputIndexes[index]).GetComponent<InputField>())
        {
            InputField input = BlockHeader.GetChild(userInputIndexes[index]).GetComponent<InputField>();
            return input.text;
        }
        else if (BlockHeader.GetChild(userInputIndexes[index]).GetComponent<Dropdown>())
        {
            Dropdown dropdown = BlockHeader.GetChild(userInputIndexes[index]).GetComponent<Dropdown>();
            return dropdown.options[dropdown.value].text;
        }
        else
        {
            BEBlock operation = BlockHeader.GetChild(userInputIndexes[index]).GetComponent<BEBlock>();
            operation.InitializeInputs();
            BEInstruction instruction = beController.GetInstruction(operation.name);
            return instruction.BEOperation(beTargetObject, operation);
        }
    }

    /// <summary>
    /// Methor that parse the inputs to string and number and return the type used by the Engine
    /// </summary>
    /// <returns>Inputs + isString flag</returns>
    public BEInputs GetAllUserInputs()
    {
        string[] stringValues = new string[userInputIndexes.Count];
        float[] numberValues = new float[userInputIndexes.Count];
        bool isString = false;
        for (int i = 0; i < userInputIndexes.Count; i++)
        {
            string value = GetDynamicUserInput(i);

            stringValues[i] = value;
            try
            {
                numberValues[i] = float.Parse(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                numberValues[i] = 0;
                isString = true;
            }
        }

        return new BEInputs(stringValues, numberValues, isString);
    }

    /// <summary>
    /// Set the value of the user input by index.
    /// Dynamic means that the input can be of types InputField, Dropdown or an Operation block.
    /// </summary>
    /// <param name="index">index of the userinput</param>
    /// <param name="value">value in string to be set</param>
    /// <param name="isOperation">if this value is true, instantiates an Operation block correspodent to the value name</param>
    /// <returns>if isOperation, return the instantiated Operation block, else return null</returns>
    public BEBlock SetDynamicUserInput(int index, string value, bool isOperation)
    {
        if (isOperation == false)
        {
            if (BlockHeader.GetChild(userInputIndexes[index]).GetComponent<InputField>())
            {
                InputField input = BlockHeader.GetChild(userInputIndexes[index]).GetComponent<InputField>();
                input.text = value;
            }
            else if (BlockHeader.GetChild(userInputIndexes[index]).GetComponent<Dropdown>())
            {
                Dropdown dropdown = BlockHeader.GetChild(userInputIndexes[index]).GetComponent<Dropdown>();
                dropdown.value = dropdown.options.FindIndex(option => option.text == value);
            }

            return null;
        }
        else
        {
            GameObject blockInstance = Instantiate(Resources.Load(blockPrefabsPath + value, typeof(GameObject))) as GameObject;

            blockInstance.transform.localScale = Vector3.one * beController.beUIController.mainCanvas.scaleFactor;

            BEBlock block = blockInstance.GetComponent<BEBlock>();
            blockInstance.name = value;

            BlockHeader.GetChild(userInputIndexes[index]).GetComponent<UIDrop>().SetBlockAtIndex(block, 1);

            return block;
        }
    }
}

/// <summary>
/// Class that define the inputs used by the Engine.
/// This types stores the value in string or float to be used by the BE Instructions, also has a helper isString flag.
/// </summary>
public class BEInputs
{
    public bool isString;
    public string[] stringValues;
    public float[] numberValues;

    public BEInputs(string[] stringValues, float[] numberValues, bool isString)
    {
        this.stringValues = stringValues;
        this.numberValues = numberValues;
        this.isString = isString;
    }
}

