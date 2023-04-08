using UnityEngine;

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
        Comment
    }

    public BlockLanguage blockLanguage;
    public BlockType blockType;

    public abstract bool Validate();
}