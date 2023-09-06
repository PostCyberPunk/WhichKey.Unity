using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PCP.Tools.WhichKey
{

    [FilePath("Project/Whichkey/AssetsData", FilePathAttribute.Location.ProjectFolder)]
    internal class AssetsDataManager : ScriptableSingleton<AssetsDataManager>
    {
        [SerializeField] public float WinTimeout;
        [SerializeField] public AssetsNavData[] projectAssetsDatas = new AssetsNavData[0];

        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }
        public static void Save()
        {
            Undo.RegisterCompleteObjectUndo(instance, "Save WhichKey AssetsData Settings");
            instance.Save(true);
        }
        public SerializedObject GetSerializedObject()
        {
            return new SerializedObject(this);
        }
    }
}
