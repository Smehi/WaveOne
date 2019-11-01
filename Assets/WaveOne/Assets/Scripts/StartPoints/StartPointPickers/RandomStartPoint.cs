using UnityEngine;

namespace SemihOrhan.WaveOne.StartPoints.StartPointPickers
{
    [RequireComponent(typeof(IStartPoint))]
    public class RandomStartPoint : MonoBehaviour, IStartPointPicker
    {
        private int size;

        public void SetListSize(int size)
        {
            this.size = size;
        }

        public int GetIndex()
        {
            return Random.Range(0, size);
        }
    }
}