using UnityEditor;
using UnityEngine;

namespace BlackTailsUnityTools.Runtime
{
    public static class SettingsProviderCreator
    {
        [MenuItem("BlackTailsTools/Create settings provider in resources")]
        static void CreateEnemies()
        {
            var resourcesPath = "Assets/Resources";
            var settingsProvider = ScriptableObject.CreateInstance<SettingsProvider>();
            if (!AssetDatabase.IsValidFolder(resourcesPath))
                AssetDatabase.CreateFolder("Assets", "Resources");
            
            AssetDatabase.CreateAsset(settingsProvider, $"Assets/Resources/SettingsProvider.asset");
        }
    }
}