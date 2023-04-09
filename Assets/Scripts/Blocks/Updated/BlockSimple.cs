using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSimple : Block
{
    public BlockSimple() {
        blockType = BlockType.Simple;
    }

    public override bool Validate() {


        return false;
    }

}
