using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
enum BlockTypeItems {
    trigger,
    simple, 
    loop, 
    conditional, 
    operation
}

public class Blocks : MonoBehaviour
{
    //trigger will be parent 
    //loop will be parent 
    //operation will be child
    //simple will be child
    BlockTypeItems blocksType;

    public void ValidateBlocks()
    {
        if(blocksType == BlockTypeItems.operation)
        {
            //get the value of child 

        }
        else if(blocksType == BlockTypeItems.simple)
        {

        }


    }


}
