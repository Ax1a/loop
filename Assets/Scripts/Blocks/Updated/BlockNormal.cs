using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNormal : Block
{
    public Transform childContainer;

    public BlockNormal() {
        blockType = BlockType.NormalBlock;
    }

    public override bool Validate() {


        return false;
    }

    public override void OnSnap(Transform closestChild, Transform parent, GameObject setGameObject, GameObject currentDrag)
    {
        Transform _closestChild;
        Block blockData = closestChild.GetComponent<Block>();
        
        if (blockData != null) {
            // Add checking of block type 
            if (blockData.blockType == Block.BlockType.Setup) {
                // if (blockData.child)
                Transform childContainer = blockData.GetComponent<BlockSetup>().childContainer;

                if (childContainer != null) {
                    // _closestChild =closestChild.GetComponent<BlockDrag>().GetClosestChild
                    setGameObject.transform.SetParent(childContainer, false);
                    Debug.Log(childContainer.GetSiblingIndex());
                    setGameObject.transform.SetSiblingIndex(childContainer.GetSiblingIndex());
                }
            }
        }

        if (blockData == null) return;

        // if (blockData.blockType == Block.BlockType.Setup) {
        //     BlockSetup blockSetup = closestChild.GetComponent<BlockSetup>();

        //     if (blockSetup.childContainer != null)
        //         setGameObject.transform.SetParent(blockSetup.childContainer, false);
        //         // Debug.Log(blockSetup.childContainer.GetSiblingIndex());
        //         // setGameObject.transform.SetSiblingIndex(blockSetup.childContainer.GetSiblingIndex());
        // }


        // string name = currentDrag.name;
        // if (name.StartsWith("C_Comment")) {
        //     Debug.Log(currentDrag.name);
        //     // do something
        // }
    }
}
