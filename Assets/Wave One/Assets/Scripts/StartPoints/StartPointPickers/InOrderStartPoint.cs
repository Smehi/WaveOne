using UnityEngine;

namespace SemihOrhan.WaveOne.StartPoints.StartPointPickers
{
    [RequireComponent(typeof(IStartPoint))]
    public class InOrderStartPoint : MonoBehaviour, IStartPointPicker
    {
        private int size;
        private int index;

        public void SetListSize(int size)
        {
            this.size = size;
            index = 0;
        }

        public int GetIndex()
        {
            if (index == size)
                index = 0;

            return index++;
        }
    }
}