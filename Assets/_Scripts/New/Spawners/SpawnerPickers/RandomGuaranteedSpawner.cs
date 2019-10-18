using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.Spawners.SpawnerPickers
{
    [RequireComponent(typeof(ISpawner))]
    public class RandomGuaranteedSpawner : MonoBehaviour, ISpawnerPicker
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
            // Find an enemy that hasn't been chosen before.
            do
            {
                // If all enemies have been chosen already exit out of the loop.
                if (!wasChosen.Contains(false))
                    return -1;

                index = Random.Range(0, size);
            } while (wasChosen.Contains(false) && wasChosen[index]);

            wasChosen[index] = true;

            return index;
        }
    }
}