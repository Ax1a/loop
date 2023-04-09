using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockConditional : Block
{
    public BlockConditional() {
        blockType = BlockType.Conditional;
    }

    public override bool Validate() {


        return false;
    }
}
