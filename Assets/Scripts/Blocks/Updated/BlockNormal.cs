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
}
