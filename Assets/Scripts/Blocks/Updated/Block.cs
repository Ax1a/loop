using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class Block : MonoBehaviour
{
    public int id;
    public enum BlockLanguage
    {
        C,
        Python,
        Java
    }
    public enum BlockType
    {
        Simple,
        Conditional,
        Operation,
        NormalBlock,
        Setup,
        Loop
    }

    public BlockLanguage blockLanguage;
    public BlockType blockType;
    public List<Transform> childBlockParent;

    public abstract bool Validate();
}