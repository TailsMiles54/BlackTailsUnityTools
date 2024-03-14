using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BlackTailsUnityTools.Editor
{
    public static class SettingsProviderCreator
    {
        [MenuItem("BlackTailsTools/Create settings provider in resources")]
        static void CreateDefaultSettings()
        {
            var resourcesPath = "Assets/Resources";
            var settingsPath = "Assets/Settings";
            var prefabsSettings = ScriptableObject.CreateInstance<PrefabsSettings>();
            if (!AssetDatabase.IsValidFolder(resourcesPath))
                AssetDatabase.CreateFolder("Assets", "Resources");
            
            if (!AssetDatabase.IsValidFolder(settingsPath))
                AssetDatabase.CreateFolder("Assets", "Settings");
            
            AssetDatabase.CreateAsset(prefabsSettings, $"{settingsPath}/PrefabsSettings.asset");
            
            var settingsProvider = SettingsProvider.CreateInstance(new List<ScriptableObject>()
            {
                prefabsSettings
            });
            AssetDatabase.CreateAsset(settingsProvider, $"{resourcesPath}/SettingsProvider.asset");
        }
    }
}