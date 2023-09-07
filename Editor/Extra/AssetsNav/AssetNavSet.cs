namespace PCP.Tools.WhichKey
{
    [System.Serializable]
    public struct AssetNavSet
    {
        public int Key;
        public string Hint;
        public string AssetPath;
        public AssetNavSet(int key, string path)
        {
            Key = key;
            Hint = path;
            AssetPath = path;
        }
    }
}
