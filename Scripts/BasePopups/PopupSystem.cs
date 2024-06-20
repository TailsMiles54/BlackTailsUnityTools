using System.Collections;
using System.Collections.Generic;
using BlackTailsUnityTools.Editor;
using UnityEditor;
using UnityEngine;
using SettingsProvider = BlackTailsUnityTools.Editor.SettingsProvider;

public class PopupSystem : MonoSingleton<PopupSystem>
{
    [SerializeField] private GameObject _background;
    [SerializeField] private Transform _popupParent;

    private BasePopup _currentPopup;

    public void ShowPopup<T>(T settings) where T : BasePopupSettings
    {
        if(_currentPopup == null)
        {
            var popupPrefab = SettingsProvider.Get<PrefabsSettings>().GetObject<Popup<T>>();
            var instance = Instantiate(popupPrefab, _popupParent, false);
            instance.Setup(settings);
            _currentPopup = instance;
            _background.SetActive(true);
        }
    }

    public void HidePopup()
    {
        _currentPopup.Hide();
        _currentPopup = null;
        _background.SetActive(false);
    }
    
    [ MenuItem("GameObject/BlackTailsObjects/PopupSystem")]
    private static void CreateArrowSelector()
    {
        // Create a custom game object
        GameObject go = new GameObject("PopupSystem");

        go.AddComponent<PopupSystem>();
        
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}