using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.StartPoints.StartPointPickers
{
    [RequireComponent(typeof(IStartPoint))]
    public class RandomGuaranteedStartPoint : MonoBehaviour, IStartPointPicker
    {
        private int size;
        private List<bool> wasChosen;
        private int index;

        public void SetListSize(int size)
        {
            this.size = size;
            wasChosen = new List<bool>();
            index = 0;

            for (int i = 0; i < size; i++)
                wasChosen.Add(false);
        }

        public int GetIndex()
        {
            // Find an point that hasn't been chosen before.
            do
            {
                // If all points have been chosen already exit out of the loop.
                if (!wasChosen.Contains(false))
                    return -1;

                index = Random.Range(0, size);
            } while (wasChosen.Contains(false) && wasChosen[index]);

            wasChosen[index] = true;

            return index;
        }
    }
}