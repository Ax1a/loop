using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLoop : Block
{
    public BlockLoop() {
        blockType = BlockType.Loop;
    }

    public override bool Validate() {


        return false;
    }

    public override void OnSnap(Transform closestChild, Transform parent, GameObject setGameObject, GameObject currentDrag)
    {
        // Block blockData = closestChild.GetComponent<Block>();

        // if (blockData == null) return;

        // if (blockData.blockType == Block.BlockType.Setup) {
        //     if (closestChild.GetComponent<BlockSetup>().childContainer != null)
        //         setGameObject.transform.SetParent(closestChild.GetComponent<BlockSetup>().childContainer, false);
        // }
        // Debug.Log(closestChild.name);
    }
}
