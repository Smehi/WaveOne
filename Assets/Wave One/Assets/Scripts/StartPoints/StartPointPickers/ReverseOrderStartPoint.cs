using UnityEngine;

namespace SemihOrhan.WaveOne.StartPoints.StartPointPickers
{
    [RequireComponent(typeof(IStartPoint))]
    public class ReverseOrderStartPoint : MonoBehaviour, IStartPointPicker
    {
        private int size;
        private int index;

        public void SetListSize(int size)
        {
            this.size = size;
            index = size - 1;
        }

        public int GetIndex()
        {
            if (index == -1)
                index = size - 1;

            return index--;
        }
    }
}