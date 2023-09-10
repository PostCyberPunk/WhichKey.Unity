namespace PCP.WhichKey
{
    [System.Serializable]
    public struct AssetNavSet
    {
        public WkKeySeq Key;
        public string Hint;
        public string AssetPath;
        public AssetNavSet(int key, string path)
        {
            Key = new int[] { key };
            Hint = path;
            AssetPath = path;
        }
    }
}
