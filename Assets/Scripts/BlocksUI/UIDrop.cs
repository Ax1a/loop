using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDrop : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    BEBlock beBlock;
    BEBlock droppedBlock;
    Transform onDragCanvas;
    RectTransform rectTransform;

    string templatePrefabsPath = "Prefabs/Blocks/Template/";

    public BETargetObject GetParentTargetObject(Transform target)
    {
        if (target.parent != null)
        {
            if (target.parent.GetComponent<BETargetObject>())
            {
                return target.parent.GetComponent<BETargetObject>();
            }
            else
            {
                return GetParentTargetObject(target.parent);
            }
        }
        else
        {
            return null;
        }
    }

    BETargetObject TargetObject
    {
        get
        {
            // v1.2 -return null when block does not had Target Object asociated 
            try
            {
                return transform.root.GetComponent<BEProgrammingEnv>().TargetObject;
            }
            catch
            {
                return null;
            }
        }
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        beBlock = GetComponent<BEBlock>();
        onDragCanvas = GameObject.FindGameObjectWithTag("onDragCanvas").transform;
    }

    public void SetBlockAtIndex(BEBlock droppedBlock, int index)
    {
        BETargetObject targetObject = TargetObject;// GetParentTargetObject(transform);

        if (targetObject != null)
        {
            //set the block to fitter unconstrained to enable the possibility of expanding the block on drop of Operations as input
            droppedBlock.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

            if (droppedBlock.BeController.ghostBlock.transform.parent != null)
            {
                SetBlockAtGhostPos(droppedBlock);
            }
            else if (transform.name == "ProgrammingEnv")
            {
                droppedBlock.transform.SetParent(transform);
                droppedBlock.transform.SetSiblingIndex(index);

                droppedBlock.BeBlockGroup = null;

                RectTransform droppedBlockRect = droppedBlock.GetComponent<RectTransform>();

                if (GetComponent<ScrollRect>())
                {
                    float borderLimit = 30;

                    Transform content = transform.GetChild(0).GetChild(0);
                    droppedBlock.transform.SetParent(content);
                    RectTransform contentRectTransform = content.GetComponent<RectTransform>();
                    bool resize = false;
                    float sizeX = contentRectTransform.sizeDelta.x;
                    float sizeY = contentRectTransform.sizeDelta.y;

                    if (droppedBlock.transform.localPosition.x + droppedBlockRect.rect.width > contentRectTransform.sizeDelta.x)
                    {
                        sizeX = droppedBlock.transform.localPosition.x + (droppedBlockRect.rect.width / 2) + droppedBlockRect.rect.width;
                        resize = true;
                    }
                    if (droppedBlock.transform.localPosition.x < borderLimit)
                    {
                        droppedBlock.transform.localPosition = new Vector3(borderLimit, droppedBlock.transform.localPosition.y, droppedBlock.transform.localPosition.z);
                    }
                    // v1.2 -scrollviewl now expands right and bottom if dropped block is beyond view limit
                    if (-droppedBlock.transform.localPosition.y < borderLimit)
                    {
                        droppedBlock.transform.localPosition = new Vector3(droppedBlock.transform.localPosition.x, -borderLimit, droppedBlock.transform.localPosition.z);
                    }
                    if (-droppedBlock.transform.localPosition.y + droppedBlockRect.rect.height > contentRectTransform.sizeDelta.y)
                    {
                        sizeY = -droppedBlock.transform.localPosition.y + (droppedBlockRect.rect.height / 2) + droppedBlockRect.rect.height;
                        resize = true;
                    }

                    if (resize)
                    {
                        contentRectTransform.sizeDelta = new Vector2(sizeX, sizeY);
                    }


                }

                // Block added to list
                if (!targetObject.beBlockGroupsList.Contains(droppedBlock))
                {
                    targetObject.beBlockGroupsList.Add(droppedBlock);
                }

                if (droppedBlock.blockType == BEBlock.BlockTypeItems.trigger)
                {
                    BEBlock blockGroup = droppedBlock;

                    blockGroup.beActiveBlock = droppedBlock;
                    droppedBlock.BeBlockGroup = blockGroup;
                    droppedBlock.beTargetObject = targetObject;

                }
            }
            else if (GetComponent<BEBlock>())
            {
                if (droppedBlock.blockType != BEBlock.BlockTypeItems.trigger && droppedBlock.blockType != BEBlock.BlockTypeItems.operation)
                {
                    droppedBlock.transform.SetParent(transform);
                    droppedBlock.transform.SetSiblingIndex(index);

                    // child block added to this object
                    try
                    {
                        GetComponent<BEBlock>().beChildBlocksList.Insert(index - 1, droppedBlock);
                    }
                    catch
                    {
                        if (!GetComponent<BEBlock>().beChildBlocksList.Contains(droppedBlock))
                        {
                            GetComponent<BEBlock>().beChildBlocksList.Add(droppedBlock);
                        }
                    }
                    droppedBlock.BeBlockGroup = GetComponent<BEBlock>().BeBlockGroup;

                    droppedBlock.beTargetObject = targetObject;//transform.root.GetComponent<BETargetObject>();
                }
                else
                {
                    UIDrop progEnv = GetParentProgrammingEnv(transform);
                    if (progEnv != null)
                    {
                        progEnv.SetBlockAtIndex(droppedBlock, 1000);
                    }
                    else
                    {
                        Destroy(droppedBlock.gameObject);
                    }
                }
            }
            else if (GetComponent<InputField>())
            {
                if (droppedBlock.blockType == BEBlock.BlockTypeItems.operation)
                {
                    int siblingIndex = transform.GetSiblingIndex();

                    gameObject.SetActive(false);

                    //store the child input + siblingindex to be placed back if needed
                    droppedBlock.GetComponent<UIDrag>().inactiveBlockInputs.Add(new InactiveBlockInput(siblingIndex, transform));

                    droppedBlock.transform.SetParent(transform.parent);

                    droppedBlock.transform.SetSiblingIndex(siblingIndex);

                    transform.SetAsLastSibling();

                    droppedBlock.beTargetObject = TargetObject;

                    BELayoutRebuild.LayoutRebuildParents(transform);
                }
                else
                {
                    UIDrop progEnv = GetParentProgrammingEnv(transform);
                    if (progEnv != null)
                    {
                        progEnv.SetBlockAtIndex(droppedBlock, 1000);
                    }
                    else
                    {
                        Destroy(droppedBlock.gameObject);
                    }
                }
            }

        }

        BELayoutRebuild.RebuildAll();

    }

    public void SetBlockAtGhostPos(BEBlock droppedBlock)
    {
        Transform ghostTransform = droppedBlock.BeController.ghostBlock.transform;
        droppedBlock.transform.SetParent(ghostTransform.parent);
        droppedBlock.transform.SetSiblingIndex(ghostTransform.GetSiblingIndex());

        // child block added to this object
        try
        {
            ghostTransform.parent.GetComponent<BEBlock>().beChildBlocksList.Insert(ghostTransform.GetSiblingIndex() - 2, droppedBlock);
        }
        catch
        {
            if (!ghostTransform.parent.GetComponent<BEBlock>().beChildBlocksList.Contains(droppedBlock))
            {
                ghostTransform.parent.GetComponent<BEBlock>().beChildBlocksList.Add(droppedBlock);
            }
        }

        droppedBlock.BeBlockGroup = ghostTransform.parent.GetComponent<BEBlock>().BeBlockGroup;
        droppedBlock.beTargetObject = TargetObject; 

    }

    public UIDrop GetParentProgrammingEnv(Transform missedDrop)
    {
        if (missedDrop.parent != null)
        {
            if (missedDrop.parent.name == "ProgrammingEnv")
            {
                return missedDrop.parent.GetComponent<UIDrop>();
            }
            else
            {
                return GetParentProgrammingEnv(missedDrop.parent);
            }
        }
        else
        {
            return null;
        }
    }

    // calculates the index position that the dragged object will assume. This is +1 to make the header be always at 0 pos
    public int CalculateIndex()
    {
        float headerHeight = 45; // default block height
        int count = 0;

        if (GetComponent<BEBlock>() != null)
        {
            foreach (BEBlock childBlock in beBlock.beChildBlocksList)
            {
                headerHeight += childBlock.GetComponent<RectTransform>().sizeDelta.y;
                count++;
                if (headerHeight >= (transform.position.y - Input.mousePosition.y))
                {
                    break;
                }
            }
        }

        int index = (int)(((transform.position.y - Input.mousePosition.y) * count) / headerHeight) + 1;
        if (index <= 0)
        {
            index = 1;
        }

        return index;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (BEEventSystem.SelectedBlock != null)
        {
            BEBlock droppedBlock = BEEventSystem.SelectedBlock;
            SetBlockAtIndex(droppedBlock, CalculateIndex());
            BEEventSystem.SetSelectedBlock(null);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
