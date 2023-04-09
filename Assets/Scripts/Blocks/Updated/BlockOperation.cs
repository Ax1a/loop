using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOperation : Block
{
    public BlockOperation() {
        blockType = BlockType.Operation;
    }

    public override bool Validate() {


        return false;
    }
}
