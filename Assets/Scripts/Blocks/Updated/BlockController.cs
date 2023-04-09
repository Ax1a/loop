using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    [SerializeField] private Transform environmentParent;
    [SerializeField] private Image deleteIcon;
    private List<GameObject> blocks = new List<GameObject>();
    private Sprite _trashClosed, _trashOpen;
    public List<Transform> childContainers;

    public static BlockController Instance;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }

        // Load the sprite from the asset
        _trashClosed = Resources.Load<Sprite>("Sprites/trash_closed");
        _trashOpen = Resources.Load<Sprite>("Sprites/trash_open");
    }

    // Show output on the console
    public void Run() {

    }

    // Validate all the blocks inside the environment
    public void ValidateBlocks() {

    }
    
    public void SetTrashIcon(bool isOpen) {
        deleteIcon.sprite = isOpen ? _trashOpen : _trashClosed;
    }

    // Update is called once per frame
    void Update()
    {
        // Testing)
        // if ((blocks == null && environmentParent.childCount != 0) || blocks.Count != environmentParent.childCount) {
        //     if (blocks != null) blocks.Clear();

        //     for (int i = 0; i < environmentParent.childCount; i++)
        //     {
        //         blocks.Add(environmentParent.GetChild(i).gameObject);
        //     }
        // }

        // foreach (var item in blocks)
        // {
        //     Block blockData = item.GetComponent<Block>();
        //     if (blockData != null) {
        //         Debug.Log(blockData.Validate() + " " + blockData.blockLanguage);
        //     }
        // }
        foreach (var item in childContainers)
        {
            foreach (Transform child in item)
            {
                Block blockData = child.GetComponent<Block>();
                if (blockData != null) {
                    Debug.Log(blockData.Validate() + " " + blockData.blockLanguage);
                }
            }
        }
    }
    

}
