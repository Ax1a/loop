using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickMenu : SaveLoadCode
{
    public UIDrag target;

    void Start()
    {
        //inherited from SaveLoadCode
        blocksPrefabsPath = "prefabs/Blocks/";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if(!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition)){
                gameObject.SetActive(false);
            }
        }
    }

    public void Duplicate()
    {
        UIDrop programmingEnv = target.GetParentProgrammingEnv(target.transform);

        List<string> virtualCode = new List<string>();
        virtualCode = TranslateBlockGroupToVirtualCode(target.GetComponent<BEBlock>());
        
        programmingEnv.StartCoroutine(TranslateVirtualCodeToBlocks(virtualCode, programmingEnv.transform, new Vector2(20, -20)));

        gameObject.SetActive(false);
    }
    
    public void Delete()
    {
        target.DragBlock();
        Destroy(target.gameObject);
        
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
