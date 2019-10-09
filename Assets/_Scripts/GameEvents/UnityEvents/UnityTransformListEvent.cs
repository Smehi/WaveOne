using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WaveOne.Events
{
    [System.Serializable]
    public class UnityTransformListEvent : UnityEvent<List<Transform>> { }
}
