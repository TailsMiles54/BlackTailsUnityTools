using System;
using Random = UnityEngine.Random;

namespace BlackTailsUnityTools.Scripts.Utils
{
    [Serializable]
    public class FloatRange
    {
        public float Min;
        public float Max;

        public float GetRandom() => Random.Range(Min, Max);
    }

    [Serializable]
    public class Range
    {
        public int Min;
        public int Max;

        public int GetRandom() => Random.Range(Min, Max);
    }
}