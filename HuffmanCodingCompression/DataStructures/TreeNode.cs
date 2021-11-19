namespace HuffmanCodingCompression.DataStructures
{
    public class TreeNode
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public TreeNode Left;
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public TreeNode Right;
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public char Value;

        public int Frequency;

        public TreeNode()
        {
        }

        public TreeNode(char value, TreeNode left = null, TreeNode right = null)
        {
            Left = left;
            Right = right;
            Value = value;
        }

        public bool IsLeafNode => Left is null && Right is null;
    }
}