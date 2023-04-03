[System.Serializable]
public class BlockData
{
    public int id;
    public BlockType blockType;
    public enum BlockType { NORMAL_BLOCK, OPERATION, SIMPLE, CONDITIONAL };
}
