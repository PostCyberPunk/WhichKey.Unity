using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using PCP.WhichKey.UI;

namespace PCP.WhichKey.Extra
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
                    var settings = WkExtraManager.instance.GetSerializedObject();
                    var vts = UILoader.instance;

                    var root = vts.List.CloneTree();

                    ListView assetsData = root.Q<ListView>();
                    assetsData.headerTitle = "AssetsNavData";
                    assetsData.makeItem = () => new PropertyField();
                    assetsData.BindProperty(settings.FindProperty("NavAssetsDatas"));

                    rootElement.Add(root);
                },
                deactivateHandler = () =>
                {
                    WkExtraManager.Save();
                },
                keywords = new HashSet<string>(new[] { "WhichKey", "AssetsData" })

            };

            return provider;
        }
    }
}
