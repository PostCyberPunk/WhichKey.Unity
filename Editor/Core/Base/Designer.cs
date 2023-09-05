using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Net;

namespace PCP.Tools.WhichKey
{
    [Serializable]
    public abstract class WKCmdArg
    {
        [SerializeField]
        public string ArgStr;
        [SerializeField]
        public int ArgInt;
        public abstract void Save();
        public abstract void Load();
    }
    public class WKKeySet
    {
        public int[] KeySeq = new int[0];
        public string KeyLabel = "None";
        public string Hint;
        public int CmdType;
        public string CmdArg;
		public void SetKeyLabel()
		{
			if (KeySeq.Length == 0)
				KeyLabel = "None";
			else
				KeyLabel = KeySeq.ToLabel();
		}
		public void Bind()
		{
			BindingWindow.ShowWindow((ks) =>
			{
				KeySeq = ks;
				SetKeyLabel();
			});
		}
    }
    public interface WKHintSource
    {
        String[] GetHint();
    }
    public interface WKCommand
    {
        public string Hint { get; }
        void Execute();
    }
    public interface WKCommandFactory
    {
        public int TID { get; }
        public string CommandName { get; }
        public WKCommand CreateCommand(WKKeySet keySet);
    }
    internal class LayerCommandFactory : WKCommandFactory
    {
        public int TID { get; } = 0;
        public string CommandName { get; } = "Layer";
        public WKCommand CreateCommand(WKKeySet keySet)
        {
            return new LayerCommand(keySet);
        }
    }
    internal class LayerCommand : WKCommand
    {
        public string Hint { get; }
        public string[] LayerHints { get; set; }
        private KeyNode target;
        public LayerCommand(WKKeySet keySet)
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
    internal class MenuCommand : WKCommand
    {

        public string Hint { get; }
        private string menuPath;
        public MenuCommand(WKKeySet keySet)
        {
            Hint = keySet.Hint;
            menuPath = keySet.CmdArg;
        }
        public void Execute()
        {
            if (!EditorApplication.ExecuteMenuItem(menuPath))
                WhichKeyManager.LogWarning($"Menu {menuPath} not available");
        }
    }
    internal class MenuCommandFactory : WKCommandFactory
    {
        public int TID { get; } = 1;
        public string CommandName { get; } = "Menu";
        public WKCommand CreateCommand(WKKeySet keySet)
        {
            return new MenuCommand(keySet);
        }
    }

}