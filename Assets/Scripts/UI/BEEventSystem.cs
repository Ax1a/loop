using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// BE dedicated wrapper class for managing BE events
/// </summary>  
public class BEEventSystem
{
    static BEBlock selectedBlock;
    public static BEBlock SelectedBlock { get => selectedBlock; }

    public enum EventType
    {
        simulated, user
    }

    static EventType currentEventType;
    public static EventType CurrentEventType { get => currentEventType; }

    public static void SetSelectedBlock(BEBlock beBlock, EventType beEventType_ = EventType.user)
    {
        selectedBlock = beBlock;
        currentEventType = beEventType_;
    }

    public static List<RaycastResult> RaycastAllBlocks()
    {
        //Set up the new Pointer Event
        PointerEventData m_PointerEventData = new PointerEventData(EventSystem.current);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
        //Raycast using the Graphics Raycaster and mouse click position
        EventSystem.current.RaycastAll(m_PointerEventData, results);

        return results;
    }
}
