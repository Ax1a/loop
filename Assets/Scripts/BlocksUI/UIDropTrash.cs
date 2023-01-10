using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDropTrash : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponent<BEBlock>())
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<BEBlock>().BeController.ghostBlock.transform.SetParent(null);
                Destroy(EventSystem.current.currentSelectedGameObject);
            }
        }
    }
}
