using UnityEngine;

namespace SemihOrhan.WaveOne.Spawners.SpawnerPickers
{
    [RequireComponent(typeof(ISpawner))]
    public class InOrderSpawner : MonoBehaviour, ISpawnerPicker
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
                return -1;

            return index++;
        }
    }
}