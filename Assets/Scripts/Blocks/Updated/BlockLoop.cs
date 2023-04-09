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

}
