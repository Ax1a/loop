#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SelectAllGameObj : MonoBehaviour
{
    private static List<GameObject> childGOs;
 
    [MenuItem( "Tools/Select All &a" )]
    private static void NewMenuOption() {
        GameObject obj = Selection.activeGameObject;
        Transform t = obj.transform;
        childGOs = new List<GameObject>();
        childGOs.Add( obj );
        GetAllChildren( t );
        GameObject[] gOs = childGOs.ToArray();
        Selection.objects = gOs;
    }

    static void GetAllChildren( Transform t ) {
        foreach( Transform childT in t ) {
            childGOs.Add( childT.gameObject );
            if( childT.childCount > 0 ) {
                GetAllChildren( childT );
            }
        }
    }
}
#endif