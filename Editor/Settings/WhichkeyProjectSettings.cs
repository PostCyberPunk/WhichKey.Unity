using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PCP.Tools.WhichKey
{
    [FilePath("Project/WhichkeyProjectSettings", FilePathAttribute.Location.ProjectFolder)]
    public class WhichkeyProjectSettings : ScriptableSingleton<WhichkeyProjectSettings>
    {
        [SerializeField] public bool showHintInstant = true;
        [SerializeField] public KeySet[] KeyMap;
        [SerializeField] public ProjectAssetsData[] projectAssetsDatas = new ProjectAssetsData[0];
        public static void Save()
        {
            Undo.RegisterCompleteObjectUndo(instance, "Save WhichKey Project Settings");
            instance.Save(true);
        }
        internal SerializedObject GetSerializedObject()
        {
            return new SerializedObject(this);
        }
        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }
    }

}
