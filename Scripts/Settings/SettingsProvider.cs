using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackTailsUnityTools.Editor
{
    public class SettingsProvider : ScriptableObject
    {
        [SerializeField] private List<ScriptableObject> _settingsList;
    
        private static SettingsProvider _settingsProvider;

        public static T Get<T>() where T : ScriptableObject
        {
            if (_settingsProvider == null)
            { 
                _settingsProvider = Resources.Load<SettingsProvider>("SettingsProvider");
            }
        
            return (T)_settingsProvider._settingsList.First(x => x is T);
        }
        
        public static SettingsProvider CreateInstance(List<ScriptableObject> settingsList)
        {
            var data = ScriptableObject.CreateInstance<SettingsProvider>();
            data.Init(settingsList);
            return data;
        }
        
        private void Init(List<ScriptableObject> settingsList)
        {
            this._settingsList = settingsList;
        }
    }
}