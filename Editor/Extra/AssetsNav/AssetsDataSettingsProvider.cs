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

                    var root = vts.ProjectSettings.CloneTree();

                    ListView assetsData = root.Q<ListView>("AssetsData");
                    assetsData.makeItem = () => new PropertyField();

                    root.Bind(settings);
                    rootElement.Add(root);
                },
                deactivateHandler = () =>
                {
                    AssetsDataManager.Save();
                },
                keywords = new HashSet<string>(new[] { "WhichKey","AssetsData" })

            };

            return provider;
        }
    }
}
