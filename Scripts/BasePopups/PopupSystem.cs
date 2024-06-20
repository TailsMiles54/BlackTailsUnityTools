using System.Collections;
using System.Collections.Generic;
using BlackTailsUnityTools.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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

    public void Setup(GameObject background, Transform popupParent)
    {
        _background = background;
        _popupParent = popupParent;
    }
    
    /*[MenuItem("GameObject/BlackTailsObjects/PopupSystem")]
    public void CreateArrowSelector()
    {
        var hasPopupSystem = FindObjectOfType<PopupSystem>();
        if(!hasPopupSystem)
        {
            var canvas = FindObjectOfType<Canvas>();
            if (!canvas)
            {
                var canvasGO = new GameObject("Canvas");
                canvasGO.AddComponent<Canvas>();
                canvas = canvasGO.GetComponent<Canvas>();
            }

            var popupParent = canvas.transform.Find("PopupParent");
            if (popupParent)
            {
                var popupParentGO = new GameObject("PopupParent");
                popupParent = popupParentGO.transform;
            }

            var popupBackground = canvas.transform.Find("PopupBackground");
            if (popupBackground)
            {
                var popupBackgroundGO = new GameObject("PopupParent");
                popupBackground.AddComponent<Image>();
                popupBackground = popupBackgroundGO.transform;
            }

            // Create a custom game object
            GameObject go = new GameObject("PopupSystem");

            var popupSystem = go.AddComponent<PopupSystem>();
            popupSystem.Setup(popupBackground.gameObject, popupParent);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }*/
    
    [MenuItem("GameObject/MyCategory/Custom Game Object", false, 10)]
    public void Test()
    {
        Debug.Log("test");
    }
}