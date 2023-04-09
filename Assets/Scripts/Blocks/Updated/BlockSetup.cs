using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSetup : Block
{
    public BlockSetup() {
        blockType = BlockType.Setup;
    }

    public override bool Validate() {


        return false;
    }
    
}
