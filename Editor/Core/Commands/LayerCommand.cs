using UnityEngine;

namespace PCP.Tools.WhichKey
{
    internal class LayerCommand : WKCommand
    {
        public string Hint { get; }
        public string[] LayerHints { get; set; }
        private KeyNode target;
        public LayerCommand(KeySet keySet)
        {
            Hint = keySet.Hint;
        }
        public void SetTarget(KeyNode node)
        {
            target = node;
        }
        public void Execute()
        {
            Debug.Log("LayerCommand");
        }
    }

}