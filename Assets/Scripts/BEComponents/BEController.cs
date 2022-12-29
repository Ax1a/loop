using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BEController : MonoBehaviour
{
    //---Vars---

    /// <summary>
    /// List of Target Objects on the scene
    /// </summary>
    public static List<BETargetObject> beTargetObjectList;

    /// <summary>
    /// List of variables of the Blocks Engine, used as variables for the block codes.
    /// </summary>
    private List<BEVariable> beVariableList;
    public List<BEVariable> BeVariableList { get => beVariableList; }
    public delegate void VariableListChangeEvent();
    public static event VariableListChangeEvent BeVariableListChangeEvent;

    [HideInInspector]
    /// <summary>
    /// Stack of all the possible instructions as components, similar to ALU.
    /// </summary>
    public Transform beInstructionStack;

    /// <summary>
    /// Dict from where the Instructions are called.
    /// </summary>
    private Dictionary<string, BEInstruction> beInstructionDict;

    /// <summary>
    /// List of colors to be used by any instruction
    /// </summary>
    public List<Material> beColorsList;

    /// <summary>
    /// List of sounds to be used by any instruction
    /// </summary>
    public List<Object> beSoundsList;

    [HideInInspector]
    public GameObject ghostBlock;

    public BEUIController beUIController;

    //---Methods---

    /// <summary>
    /// Get instruction from instruction stack.
    /// </summary>
    /// <param name="instructionName">Name of the instruction to be returned</param>
    /// <returns>BEInstrucion type instruction</returns>
    public BEInstruction GetInstruction(string instructionName)
    {
        return beInstructionDict[instructionName];
    }

    /// <summary>
    /// Set a value to a specified BE Variable by name.
    /// </summary>
    /// <param name="varName">Name of the BE Variable</param>
    /// <param name="varValue">value to the Variable</param>
    public void SetVariable(string varName, string varValue)
    {
        BEVariable variable = beVariableList.Find(v => v.name == varName);
        if (variable != null)
        {
            beVariableList[beVariableList.IndexOf(variable)] = new BEVariable(varName, varValue);
        }
        else
        {
            beVariableList.Add(new BEVariable(varName, varValue));
        }

        BeVariableListChangeEvent();
    }

    /// <summary>
    /// Returns the value of a specified BE Variable by its name.
    /// </summary>
    /// <param name="varName">Name of the BE Variable</param>
    /// <returns>BE Variable value</returns>
    public string GetVariable(string varName)
    {
        try
        {
            BEVariable variable = beVariableList.Find(v => v.name == varName);
            return variable.value;
        }
        catch
        {
            return "0";
        }

    }

    public Material GetColor(string colorName)
    {
        try
        {
            Material mat = (Material)beColorsList.Find(a => a.name == colorName);
            return mat;
        }
        catch
        {
            return null;
        }
    }

    public AudioClip GetSound(string soundName)
    {
        try
        {
            AudioClip sound = (AudioClip)beSoundsList.Find(a => a.name == soundName);
            return sound;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Activates all the block groups with a "ControllerMain" Trigger block.
    /// </summary>
    public void MainPlay()
    {
        foreach (BETargetObject iBP_TargetObject in beTargetObjectList)
        {
            foreach (BEBlock blockGroup in iBP_TargetObject.beBlockGroupsList)
            {
                if (blockGroup.blockType == BEBlock.BlockTypeItems.trigger)
                {
                    if (blockGroup.name == "ControllerMain" && blockGroup.beActiveBlock.beChildBlocksList.Count > 0)
                    {
                        clickTime += 1;
                        blockGroup.isActive = true;
                    }
                    Debug.Log(string.Format("PlayButtonClick={0} times", clickTime));
                }
            }
        }
    }

    /// <summary>
    /// Stops all the currently active block groups.
    /// It also stop the groups with a "When Pressed" Trigger block, but if the button is still pressed, it will be active again almost instantly.
    /// The block groups have the same type as any other block, but only Trigger blocks are placed on the beBlockGroupsList list.
    /// </summary>
    public void MainStop()
    {
        foreach (BETargetObject beTargetObject in beTargetObjectList)
        {
            foreach (BEBlock blockGroup in beTargetObject.beBlockGroupsList)
            {
                StopGroup(blockGroup);
            }
        }
    }

    /// <summary>
    /// Stop a specified block group by resetting the needed variables.
    /// </summary>
    /// <param name="blockGroup">BE Block to be stopped</param>
    public void StopGroup(BEBlock blockGroup)
    {
        blockGroup.isActive = false;
        blockGroup.beActiveBlock = blockGroup.GetComponent<BEBlock>();
        blockGroup.blockOutline.enabled = false;
    }

    /// <summary>
    /// Activates the next block of the same parent as the specified block given as input to this method. 
    /// </summary>
    /// <param name="block">Previously activated block</param>
    public void PlayNextOutside(BEBlock block)
    {
        if (block.BeBlockGroup.isActive)
        {
            BEBlock parentBlock = block.transform.parent.GetComponent<BEBlock>();

            try
            {
                BEBlock nextBlock = parentBlock.beChildBlocksList[block.transform.GetSiblingIndex()];

                block.BeBlockGroup.beActiveBlock = nextBlock;
            }
            catch
            {
                if (parentBlock.blockType == BEBlock.BlockTypeItems.loop)
                {
                    // end of the loop reached
                    block.BeBlockGroup.beActiveBlock = parentBlock;
                }
                else if (parentBlock.blockType == BEBlock.BlockTypeItems.trigger)
                {
                    // end of the group(main) reached
                    if (parentBlock.name == "ControllerMain")
                    {
                        block.BeBlockGroup.isActive = false;
                    }

                    block.BeBlockGroup.beActiveBlock = parentBlock;

                }
                else if (parentBlock.blockType == BEBlock.BlockTypeItems.conditional)
                {
                    PlayNextOutside(parentBlock);
                }
            }
        }
    }

    /// <summary>
    /// Activates the first child block of the specified block given as input to this method. 
    /// </summary>
    /// <param name="block">Previously activated block</param>
    public void PlayNextInside(BEBlock beBlock)
    {
        if (beBlock.BeBlockGroup.isActive)
        {
            if (beBlock.beChildBlocksList.Count > 0)
            {
                BEBlock nextBlock = beBlock.beChildBlocksList[0];
                beBlock.BeBlockGroup.beActiveBlock = nextBlock;
            }
        }
    }

    private void Start()
    {
        beUIController.GetComponent<BEUIController>();
    }

    /// <summary>
    /// Update the beTargetObjectList with all Target Objects on scene.
    /// </summary>
    public void FindTargetObjects()
    {
        BETargetObject[] tgtObj = FindObjectsOfType<BETargetObject>();
        beTargetObjectList = new List<BETargetObject>();
        beTargetObjectList.AddRange(tgtObj);
    }

    public string templatePrefabsPath;

    void Awake()
    {

        if (name == "BEController")
        {
            templatePrefabsPath = "prefabs/Blocks/Template/";

            foreach (Transform child in transform)
            {
                if (child.name == "BEInstructionStack")
                {
                    beInstructionStack = child;
                }
            }
            BEInstruction[] instructions = beInstructionStack.GetComponents<BEInstruction>();

            beInstructionDict = new Dictionary<string, BEInstruction>();
            beInstructionDict.Clear();
            foreach (BEInstruction instruction in instructions)
            {
                beInstructionDict.Add(instruction.GetType().ToString(), instruction);
            }

        }

        FindTargetObjects();

        // initializing the variables list with an example variable called "Variable"
        beVariableList = new List<BEVariable>
        {
            new BEVariable("Variable", "0")
        };

        // pupulate the colors list
        beColorsList = new List<Material>();
        Material[] colors = Resources.LoadAll<Material>("Colors");
        beColorsList.AddRange(colors);

        //populate the sounds list
        beSoundsList = new List<Object>();
        //finding all the clips
        try
        {
            Object[] myClips = Resources.LoadAll<AudioClip>("Sounds");
            beSoundsList.AddRange(myClips);
        }
        catch
        {
            Debug.Log("couldn't load sounds");
        }

    }

    List<GameObject> fieldUnderPointer = new List<GameObject>();

    void Update()
    {
        if (BEEventSystem.SelectedBlock != null && BEEventSystem.CurrentEventType == BEEventSystem.EventType.user)
        {
            BEBlock draggedBlock = BEEventSystem.SelectedBlock;//EventSystem.current.currentSelectedGameObject.GetComponent<BEBlock>();

            foreach (RaycastResult result in BEEventSystem.RaycastAllBlocks())
            {
                if (result.gameObject.transform.root.tag != "GameController")
                {
                    if (result.gameObject.GetComponent<BEBlock>())
                    {
                        if (result.gameObject.GetComponent<UIDrop>())
                        {
                            UIDrop toDropBlock = result.gameObject.GetComponent<UIDrop>();

                            if (draggedBlock.blockType != BEBlock.BlockTypeItems.trigger && draggedBlock.blockType != BEBlock.BlockTypeItems.operation)
                            {
                                for (int i = fieldUnderPointer.Count - 1; i >= 0; i--)
                                {
                                    fieldUnderPointer[i].GetComponent<Outline>().enabled = false;
                                    fieldUnderPointer.Remove(fieldUnderPointer[i]);
                                }
                                Transform newParent = result.gameObject.transform;
                                if (ghostBlock.transform.parent != newParent)
                                {
                                    BELayoutRebuild.RebuildAll();
                                }

                                ghostBlock.transform.SetParent(newParent);
                                ghostBlock.transform.SetSiblingIndex(toDropBlock.CalculateIndex());

                                break;
                            }
                        }
                    }
                    else if (result.gameObject.GetComponent<InputField>())
                    {
                        UIDrop toDropField = result.gameObject.GetComponent<UIDrop>();

                        if (draggedBlock.blockType == BEBlock.BlockTypeItems.operation)
                        {
                            if (!fieldUnderPointer.Contains(result.gameObject))
                                fieldUnderPointer.Add(result.gameObject);

                            result.gameObject.GetComponent<Outline>().enabled = true;
                            break;
                        }
                    }
                    else if (result.gameObject.name == "ProgrammingEnv")
                    {
                        for (int i = fieldUnderPointer.Count - 1; i >= 0; i--)
                        {
                            fieldUnderPointer[i].GetComponent<Outline>().enabled = false;
                            fieldUnderPointer.Remove(fieldUnderPointer[i]);
                        }
                        ghostBlock.transform.SetParent(null);
                        break;
                    }
                }
            }
        }

        // only one object should have a BEController component attached 
        //if (name == "BEController")
        //{
        foreach (BETargetObject beTargetObject in beTargetObjectList)
        {
            foreach (BEBlock blockGroup in beTargetObject.beBlockGroupsList)
            {
                if (blockGroup.blockType == BEBlock.BlockTypeItems.trigger)
                {
                    blockGroup.beActiveBlock.InitializeInputs();

                    BEInstruction instruction = GetInstruction(blockGroup.beActiveBlock.name);
                    instruction.BEFunction(beTargetObject, blockGroup.beActiveBlock);
                }
            }
        }
        //}
    }

    //--- Custom inspector ---
    // Scene section
    [HideInInspector]
    public bool singleEnabledProgrammingEnv = false;

#if UNITY_EDITOR

    // Workshop section
    string blocksPrefabsPath;

    [HideInInspector]
    public string targetObjectsPrefabsPath;
    [HideInInspector]
    public Vector3 newTargetObjectPosition;
    [HideInInspector]
    public BEBlock.BlockTypeItems newBlockType;
    [HideInInspector]
    public string newBlockHeaderGuideline;
    [HideInInspector]
    public string newBlockInstructionName;
    [HideInInspector]
    public Color newBlockColor = Color.green;
    [HideInInspector]
    public bool newBlockCreated = false;
    [HideInInspector]
    public string importLog;
    public int clickTime = 0;

    public void BuildTargetObject(string name)
    {
        targetObjectsPrefabsPath = "prefabs/TargetObjects/";
        GameObject newTargetObject = Instantiate(Resources.Load(targetObjectsPrefabsPath + name, typeof(GameObject))) as GameObject;
        newTargetObject.transform.position = newTargetObjectPosition;
        newTargetObject.name = name;

        //v1.1 -On creating target object via workshop, if in play mode, add newly created targed object to controller list
        if (UnityEditor.EditorApplication.isPlaying)
        {
            FindTargetObjects();
        }
    }

    public void BuildBlock(string headerGuideline, string instructionName, BEBlock.BlockTypeItems type, Color color)
    {
        templatePrefabsPath = "prefabs/Blocks/Template/";

        GameObject newBlockGameObject = Instantiate(Resources.Load(templatePrefabsPath + "BlockTemplate", typeof(GameObject))) as GameObject;
        BEBlock newBlock = newBlockGameObject.GetComponent<BEBlock>();
        newBlockGameObject.transform.SetParent(beUIController.BlocksPanel);
        newBlockGameObject.transform.SetAsFirstSibling();
        newBlock.blockType = type;

        string instructionNameClear = System.Text.RegularExpressions.Regex.Replace(instructionName, "[^\\w\\._]", "");
        instructionNameClear = instructionNameClear.Replace(" ", "");

        newBlockGameObject.name = instructionNameClear;
        newBlock.SetBlocksLayout();
        for (int i = newBlock.transform.GetChild(0).childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(newBlock.transform.GetChild(0).GetChild(i).gameObject);
        }
        newBlockGameObject.GetComponent<Image>().color = color;

        string[] headerObjects = headerGuideline.Split('\n');

        foreach (string objString in headerObjects)
        {
            string tempObjString = "";
            foreach (char c in objString)
            {
                if (!char.IsControl(c))
                    tempObjString += c;
            }
            tempObjString = tempObjString.TrimStart(' ').TrimEnd(' ');
            if (tempObjString != "")
            {
                try
                {
                    GameObject headerObj;
                    if (tempObjString.ToLower().Contains("[inputfield") && tempObjString.Substring(tempObjString.Length - 1, 1) == "]")
                    {
                        headerObj = Instantiate(Resources.Load(templatePrefabsPath + "inputfield", typeof(GameObject))) as GameObject;
                        headerObj.name = "InputField";
                        string value;
                        if (tempObjString.Contains("="))
                        {
                            value = tempObjString.Split('=')[1].TrimStart(' ').TrimEnd(']', ' ');
                        }
                        else
                        {
                            value = "";
                        }
                        headerObj.GetComponent<InputField>().text = value;
                    }
                    else if (tempObjString.ToLower().Contains("[dropdown") && tempObjString.Substring(tempObjString.Length - 1, 1) == "]")
                    {
                        headerObj = Instantiate(Resources.Load(templatePrefabsPath + "dropdown", typeof(GameObject))) as GameObject;
                        headerObj.name = "Dropdown";
                        if (tempObjString.Contains("="))
                        {
                            string[] options = tempObjString.Split('=')[1].TrimStart(' ').TrimEnd(']', ' ').Split(',');
                            Dropdown dropdown = headerObj.GetComponent<Dropdown>();
                            dropdown.ClearOptions();
                            foreach (string option in options)
                            {
                                string tempOption = option.TrimStart(' ').TrimEnd(' ');
                                dropdown.options.Add(new Dropdown.OptionData(tempOption));
                            }
                            dropdown.RefreshShownValue();
                        }
                    }
                    else
                    {
                        headerObj = Instantiate(Resources.Load(templatePrefabsPath + "text", typeof(GameObject))) as GameObject;
                        headerObj.name = "Text";
                        headerObj.GetComponent<Text>().text = tempObjString.TrimStart(' ');
                    }

                    headerObj.transform.SetParent(newBlock.transform.GetChild(0));
                    headerObj.transform.SetAsLastSibling();

                }
                catch
                {
                    Debug.Log("Couldn't identify header guideline");
                }
            }
        }

        string localPath = "Assets/PlayModeBlocksEngine/resources/prefabs/Blocks/" + newBlockGameObject.name + ".prefab";
        UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(newBlockGameObject, localPath, UnityEditor.InteractionMode.UserAction);

        CreateInstruction(instructionNameClear);

        beInstructionStack = transform.GetChild(0);

        LayoutRebuilder.ForceRebuildLayoutImmediate(beUIController.BlocksPanel.GetComponent<RectTransform>());
    }

    public void CreateInstruction(string instructionName)
    {
        string instructionsPath = Application.dataPath + "/PlayModeBlocksEngine/Scripts/BEComponents/BEInstructions/";

        string fullPath = instructionsPath + instructionName + ".cs";

        if (File.Exists(fullPath) == false)
        {
            using (StreamWriter file =
                new StreamWriter(fullPath))
            {
                file.WriteLine("using UnityEngine;");
                file.WriteLine("using System.Collections;");
                file.WriteLine("");
                file.WriteLine("public class " + instructionName + " : BEInstruction");
                file.WriteLine("{");
                file.WriteLine(" ");
                file.WriteLine("\t// Use this for Operations");
                file.WriteLine("\tpublic override string BEOperation(BETargetObject targetObject, BEBlock beBlock)");
                file.WriteLine("\t{");
                file.WriteLine("\t\tstring result = \"0\";");
                file.WriteLine("\t\t");
                file.WriteLine("\t\t// Use \"beBlock.BeInputs\" to get the input values");
                file.WriteLine("\t\t");
                file.WriteLine("\t\treturn result;");
                file.WriteLine("\t}");
                file.WriteLine("\t");
                file.WriteLine("\t// Use this for Functions");
                file.WriteLine("\tpublic override void BEFunction(BETargetObject targetObject, BEBlock beBlock)");
                file.WriteLine("\t{");
                file.WriteLine("\t\t// Use \"beBlock.BeInputs\" to get the input values");
                file.WriteLine("\t\t");
                file.WriteLine("\t\t// Make sure to end the function with a \"BeController.PlayNextOutside\" method and use \"BeController.PlayNextInside\" to play child blocks if needed");
                file.WriteLine("\t\tBeController.PlayNextOutside(beBlock);");
                file.WriteLine("\t}");
                file.WriteLine(" ");
                file.WriteLine("}");
            }//File written

        }
        else
        {
            Debug.Log("File already exists");
        }
        UnityEditor.AssetDatabase.Refresh();

    }


    /// <summary>
    /// Try to get type from assemblies.
    /// From: https://stackoverflow.com/a/11811046
    /// </summary>
    /// <param name="typeName">Type name</param>
    /// <returns>Type or null</returns>
    public System.Type TryGetType(string typeName)
    {
        var type = System.Type.GetType(typeName);
        if (type != null) return type;
        foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null)
                return type;
        }
        return null;
    }

    /// <summary>
    /// Try to add component in the Isntruction Stack by instruction name as string
    /// </summary>
    /// <param name="beInstructionStack">Instruction Stack</param>
    /// <param name="instructionName">Type name</param>
    public void TryAddComponent(string instructionName)
    {
        instructionName = System.Text.RegularExpressions.Regex.Replace(instructionName, "[^\\w\\._]", "");
        instructionName = instructionName.Replace(" ", "");

        if (instructionName != "")
        {
            if (TryGetType(instructionName) == null)
            {
                UnityEditor.EditorGUILayout.HelpBox("Block and Instruction created.\nTrying to import Instruction to stack...", UnityEditor.MessageType.Info);

                if (GUILayout.Button("Stop importing"))
                {
                    newBlockCreated = false;
                }
            }
            else
            {
                if (beInstructionStack.GetComponent(instructionName) == null)
                {
                    beInstructionStack.gameObject.AddComponent(TryGetType(instructionName));
                }
                else
                {
                    Debug.Log("Instruction already in the stack");
                }

                newBlockCreated = false;
            }
        }
    }

    public string ReimportInstructions()
    {
        importLog = "";
        int countSuccess = 0;

        string instructionsPath = Application.dataPath + "/PlayModeBlocksEngine/Scripts/BEComponents/BEInstructions/";
        string blocksPath = Application.dataPath + "/PlayModeBlocksEngine/resources/prefabs/Blocks/";
        //blocksPrefabsPath = "prefabs/Blocks/";
        beInstructionStack = transform.GetChild(0);
        FileInfo instructionScript;
        DirectoryInfo directory = new DirectoryInfo(blocksPath);
        FileInfo[] prefabs = directory.GetFiles("*.prefab");

        importLog += "--- Importing from " + prefabs.Length + " Blocks ---";

        foreach (FileInfo prefab in prefabs)
        {
            string prefabName = Path.GetFileNameWithoutExtension(prefab.FullName);

            importLog += "\n" + prefabName + ": ";

            instructionScript = new FileInfo(instructionsPath + prefabName + ".cs");

            if (instructionScript.Exists)
            {
                if (beInstructionStack.GetComponent(prefabName) == null)
                {
                    beInstructionStack.gameObject.AddComponent(TryGetType(prefabName));
                    importLog += "Success - Imported";
                    countSuccess++;
                }
                else
                {
                    importLog += "Success - Instruction already in the stack";
                    countSuccess++;
                }
            }
            else
            {
                importLog += "Failed - Instruction script doesn't exists or not in the Scripts/BEComponents/BEInstructions folder";
            }
        }

        importLog += "\nSuccess: " + countSuccess + "\nFailed: " + Mathf.Abs(prefabs.Length - countSuccess);
        importLog += "\n--- End of Log ---";

        return importLog;
    }

#endif
    //------------------------------
}

/// <summary>
/// Define the BE Variable object with name and value (string).
/// </summary>
public class BEVariable
{
    public string name;
    public string value;

    public BEVariable(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
}

/// <summary>
/// Define the Instructions that are called by the BE Controller.
/// </summary>
public class BEInstruction : MonoBehaviour
{
    private static BEController beController;
    public static BEController BeController { get => beController; }

    private void Start()
    {
        try
        {
            beController = transform.parent.GetComponent<BEController>();
        }
        catch { }
    }

    /// <summary>
    /// Operation Block behavior wrapper method.
    /// It can be userd to set conditions returning "1"(true) or "0"(false) to conditional blocks or just implement
    /// the condition inside the BEFunction().
    /// OBS.: All the operations return string and the BEFunctions are written to deal with string or number in a generic manner.
    /// </summary>
    /// <param name="beTargetObject">Target Object</param>
    /// <param name="beBlock">Block</param>
    /// <returns></returns>
    public virtual string BEOperation(BETargetObject beTargetObject, BEBlock beBlock)
    {
        //override and implement inside each block that needs it

        return "0";
    }

    /// <summary>
    /// Function Blocks behavior wrapper method.
    /// Used to setup the behaviour that are going to be transmitted for the Target Object.
    /// </summary>
    /// <param name="beTargetObject">Target Object</param>
    /// <param name="beBlock">Block</param>
    public virtual void BEFunction(BETargetObject beTargetObject, BEBlock beBlock)
    {
        //override and implement inside each block that needs it
    }
}