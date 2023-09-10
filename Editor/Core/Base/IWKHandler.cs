namespace PCP.WhichKey.Types
{
    public interface IWKHandler
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">key to handle</param>
        /// <returns>true if you want to close hint window</returns>
        void ProcessKey(int key);

        /// <summary>
        /// the Hints you want to show in hint window
        /// </summary>
        /// <returns>odd elements are key,even elments are hint</returns>
        string[] GetLayerHints();

    }
}
