using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BEProgrammingEnv : MonoBehaviour
{
    BETargetObject targetObject;
    public BETargetObject TargetObject { get => targetObject; }

    // v1.3 -Moved from Start() to Awake() to set targetObject.beProgrammingEnv before BEUIController runs Rescale()
    void Awake()
    {
        targetObject = transform.parent.GetComponent<BETargetObject>();
        targetObject.beProgrammingEnv = this;
        transform.SetParent(null);

        // v1.3 -set start position of the programming environment content to top-left
        RectTransform contentRectT = GetComponentInChildren<ScrollRect>().content.GetComponent<RectTransform>();
        contentRectT.offsetMax = new Vector2(0, 0);

        // v1.2 -Custom UI Scale section on the inspector for adjusting the scale based on the screen width
        GetComponent<Canvas>().scaleFactor = targetObject.BeController.beUIController.uiScale; 
        
        // v1.0.1 -Bug fix: null when getting programming environment from BEProgrammingEnv
        GetComponentInChildren<SaveLoadCode>().beTargetObject = TargetObject;
    }
}
