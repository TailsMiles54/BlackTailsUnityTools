using System.Collections;
using System.Collections.Generic;
using BlackTailsUnityTools.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SettingsProvider = BlackTailsUnityTools.Editor.SettingsProvider;

public class PopupSystem : MonoSingleton<PopupSystem>
{
    [SerializeField] private GameObject _background;
    [SerializeField] private Transform _popupParent;

    private BasePopup _currentPopup;
    private const string kUILayerName = "UI";

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
    
    [MenuItem("GameObject/BlackTailsObjects/PopupSystem")]
    private static void CreateArrowSelector(MenuCommand menuCommand)
    {
        var hasPopupSystem = FindObjectOfType<PopupSystem>();
        if(!hasPopupSystem)
        {
            var canvas = FindObjectOfType<Canvas>();
            if (!canvas)
            {
                var go = CreateNewUI();
                SetParentAndAlign(go, menuCommand.context as GameObject);
                if (go.transform.parent as RectTransform)
                {
                    RectTransform rect = go.transform as RectTransform;
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;
                    rect.anchoredPosition = Vector2.zero;
                    rect.sizeDelta = Vector2.zero;
                }
                Selection.activeGameObject = go;
                canvas = go.GetComponent<Canvas>();
            }

            var popupBackground = canvas.transform.Find("PopupBackground").GameObject();
            if (!popupBackground)
            {
                popupBackground = new GameObject("PopupBackground");
                var image = popupBackground.AddComponent<Image>();
                //var backgroundImage = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
                //image.sprite = backgroundImage;

                image.type = Image.Type.Sliced;
                
                RectTransform rect = popupBackground.transform as RectTransform;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = Vector2.zero;
                
                SetParentAndAlign(popupBackground, canvas.gameObject);

                /*Left*/ rect.offsetMin = Vector2.zero;
                /*Right*/ rect.offsetMax = Vector2.zero;
                /*Top*/ rect.offsetMax = Vector2.zero;
                /*Bottom*/ rect.offsetMin = Vector2.zero;

                image.color = new Color(0f, 0f, 0f, 0.5f);
                
                Undo.RegisterCreatedObjectUndo(popupBackground, "Create " + popupBackground.name);
                Selection.activeObject = popupBackground;
            }

            var popupParent = canvas.transform.Find("PopupParent").GameObject();
            if (!popupParent)
            {
                popupParent = new GameObject("PopupParent");
                SetParentAndAlign(popupParent, canvas.gameObject);
                Undo.RegisterCreatedObjectUndo(popupParent, "Create " + popupParent.name);
                Selection.activeObject = popupParent;
            }

            // Create a custom game object
            GameObject popupSystemgo = new GameObject("PopupSystem");

            
            Debug.Log(popupBackground + " " + popupParent);
            var popupSystem = popupSystemgo.AddComponent<PopupSystem>();
            popupSystem.Setup(popupBackground.gameObject, popupParent.transform);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(popupSystemgo, "Create " + popupSystemgo.name);
            Selection.activeObject = popupSystemgo;
        }
    }
    
    private static GameObject CreateNewUI()
    {
        // Root for the UI
        var root = ObjectFactory.CreateGameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        root.layer = LayerMask.NameToLayer(kUILayerName);
        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Works for all stages.
        StageUtility.PlaceGameObjectInCurrentStage(root);
        bool customScene = false;
        PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            Undo.SetTransformParent(root.transform, prefabStage.prefabContentsRoot.transform, "");
            customScene = true;
        }

        Undo.SetCurrentGroupName("Create " + root.name);

        // If there is no event system add one...
        // No need to place event system in custom scene as these are temporary anyway.
        // It can be argued for or against placing it in the user scenes,
        // but let's not modify scene user is not currently looking at.
        if (!customScene)
            CreateEventSystem(false);
        return root;
    }

    private static void CreateEventSystem(bool select)
    {
        CreateEventSystem(select, null);
    }

    private static void CreateEventSystem(bool select, GameObject parent)
    {
        StageHandle stage = parent == null ? StageUtility.GetCurrentStageHandle() : StageUtility.GetStageHandle(parent);
        var esys = stage.FindComponentOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = ObjectFactory.CreateGameObject("EventSystem");
            if (parent == null)
                StageUtility.PlaceGameObjectInCurrentStage(eventSystem);
            else
                SetParentAndAlign(eventSystem, parent);
            esys = ObjectFactory.AddComponent<EventSystem>(eventSystem);
            ObjectFactory.AddComponent<StandaloneInputModule>(eventSystem);

            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }

        if (select && esys != null)
        {
            Selection.activeGameObject = esys.gameObject;
        }
    }

    private static void SetParentAndAlign(GameObject child, GameObject parent)
    {
        if (parent == null)
            return;

        Undo.SetTransformParent(child.transform, parent.transform, "");

        RectTransform rectTransform = child.transform as RectTransform;
        if (rectTransform)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            Vector3 localPosition = rectTransform.localPosition;
            localPosition.z = 0;
            rectTransform.localPosition = localPosition;
        }
        else
        {
            child.transform.localPosition = Vector3.zero;
        }
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;

        SetLayerRecursively(child, parent.layer);
    }

    private static void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        Transform t = go.transform;
        for (int i = 0; i < t.childCount; i++)
            SetLayerRecursively(t.GetChild(i).gameObject, layer);
    }
}