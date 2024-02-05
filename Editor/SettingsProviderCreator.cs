using UnityEditor;
using UnityEngine;

namespace BlackTailsUnityTools.Runtime
{
    public static class SettingsProviderCreator
    {
        [MenuItem("BlackTailsTools/Create settings provider in resources")]
        static void CreateEnemies()
        {
            var settingsProvider = ScriptableObject.CreateInstance<SettingsProvider>();
            AssetDatabase.CreateAsset(settingsProvider, $"Assets/Resources/SettingsProvider.asset");
        }
    }
}