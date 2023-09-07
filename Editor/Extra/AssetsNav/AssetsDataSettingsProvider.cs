using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace PCP.Tools.WhichKey
{

    static class AssetsDataSettingsProvider
    {

        public const string settingsPath = "Project/WhichKey/AssetsNavigation ";

        [SettingsProvider]
        public static SettingsProvider CreateProjectSettings()
        {
            // WhichKey.instance.Save();

            var provider = new SettingsProvider(settingsPath, SettingsScope.Project)
            {
                label = "AssetsNavigation",
                activateHandler = (searchContext, rootElement) =>
                {
                    var settings = AssetsDataManager.instance.GetSerializedObject();
                    var vts = WhichKeyManager.mUILoader;

                    var root = vts.List.CloneTree();

                    ListView assetsData = root.Q<ListView>();
                    assetsData.headerTitle = "AssetsData";
                    assetsData.makeItem = () => new PropertyField();
                    assetsData.BindProperty(settings.FindProperty("NavAssetsDatas"));

                    rootElement.Add(root);
                },
                deactivateHandler = () =>
                {
                    AssetsDataManager.Save();
                },
                keywords = new HashSet<string>(new[] { "WhichKey", "AssetsData" })

            };

            return provider;
        }
    }
}
