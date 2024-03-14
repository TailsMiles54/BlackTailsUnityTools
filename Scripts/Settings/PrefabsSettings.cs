using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackTailsUnityTools.Editor
{
    public class PrefabsSettings : ScriptableObject
    {
        [SerializeField] private List<MonoBehaviour> _gameObjects;
        
        public T GetObject<T>() where T : MonoBehaviour
        {
            try
            {
                return (T)_gameObjects.First(x => x is T);
            }
            catch (Exception e)
            {
                Debug.LogWarning(typeof(T));
                throw;
            }
        }
    }
}