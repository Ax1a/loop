using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BE dedicated wrapper class to manage layout changes
/// </summary>
public class BELayoutRebuild
{
    static int refreshCounter;

    public static void RebuildAll()
    {
        refreshCounter = 0;
    }

    public static IEnumerator DelayedLayoutRebuild(BEBlock beBlock)
    {
        refreshCounter = 0;
        while (true)
        {
            if (refreshCounter < 100)
            {
                refreshCounter++;
                beBlock.rectTransform.sizeDelta = new Vector2(beBlock.BlockHeader.GetComponent<HorizontalLayoutGroup>().preferredWidth + 25, beBlock.rectTransform.sizeDelta.y);
                LayoutRebuilder.ForceRebuildLayoutImmediate(beBlock.rectTransform);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public static void LayoutRebuildParents(Transform blockTransform)
    {
        if (blockTransform.parent != null)
        {
            if (blockTransform.parent.GetComponent<BEBlock>())
            {
                BEBlock parentBlock = blockTransform.parent.GetComponent<BEBlock>();
                RectTransform parentRect = parentBlock.GetComponent<RectTransform>();
                parentRect.sizeDelta = new Vector2(parentBlock.BlockHeader.GetComponent<HorizontalLayoutGroup>().preferredWidth + 25, parentRect.sizeDelta.y);
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
            }
            else
            {
                LayoutRebuildParents(blockTransform.parent);
            }
        }
    }
}
