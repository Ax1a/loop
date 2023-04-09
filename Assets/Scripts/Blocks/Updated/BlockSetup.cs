using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSetup : Block
{
    public Transform childContainer;
    public BlockSetup() {
        blockType = BlockType.Setup;
    }

    public override bool Validate() {


        return false;
    }

    public override void OnSnap(Transform closestChild, Transform parent, GameObject setGameObject, GameObject currentDrag)
    {
        setGameObject.transform.SetParent(parent.transform, false);
        setGameObject.transform.SetSiblingIndex(closestChild.GetSiblingIndex());
    }

    
}
