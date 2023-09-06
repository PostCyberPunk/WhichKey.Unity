namespace PCP.Tools.WhichKey
{
    [System.Serializable]
    public struct AssetData
    {
        public int Key;
        public string Hint;
        public string AssetPath;
        public AssetData(int key, string path)
        {
            Key = key;
            Hint = "";
            AssetPath = path;
        }
    }
}
