using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace PCP.Tools.WhichKey
{
    [FilePath("Project/WhichkeyProjectSettings", FilePathAttribute.Location.ProjectFolder)]
    internal class WhichkeyProjectSettings : ScriptableSingleton<WhichkeyProjectSettings>
    {
        [SerializeField] public KeySet[] LayerMap = new KeySet[0];
        [SerializeField] public KeySet[] MenuMap = new KeySet[0];
        [SerializeField] public KeySet[] KeyMap = new KeySet[0];
        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }
        public static void Save()
        {
            Undo.RegisterCompleteObjectUndo(instance, "Save WhichKey Project Settings");
            instance.Save(true);
        }
        public SerializedObject GetSerializedObject()
        {
            return new SerializedObject(this);
        }
        public void Apply()
        {
            Save();
            WhichKeyManager.instance.Refresh();
        }

    }
}
