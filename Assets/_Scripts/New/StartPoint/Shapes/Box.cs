using UnityEngine;

namespace WaveOne.StartPoints.Shapes
{
    [System.Serializable]
    public struct Box
    {
        public Vector3 position;
        public Vector3 size;
        public Vector3 minDistanceFromCenter;
    }
}
