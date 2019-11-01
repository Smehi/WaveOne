using UnityEngine;

namespace SemihOrhan.WaveOne.StartPoints.Shapes
{
    [System.Serializable]
    public struct Box
    {
        public Transform transform;
        public Vector3 size;
        public Vector3 minDistanceFromCenter;
    }
}
