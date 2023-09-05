using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCP.Tools.WhichKey
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
        /// <returns>odd elements are key,even elments are hint</returns> <summary>
        string[] GetLayerHints();

    }
}
