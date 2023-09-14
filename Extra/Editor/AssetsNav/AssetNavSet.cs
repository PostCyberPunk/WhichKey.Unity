using PCP.WhichKey.Types;
using System.Linq;
namespace PCP.WhichKey.Extra
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
            Hint = path.Split("/").Last();
            AssetPath = path;
        }
    }
}
