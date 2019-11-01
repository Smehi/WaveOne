using UnityEngine;

namespace SemihOrhan.WaveOne.Spawners.SpawnerPickers
{
    [RequireComponent(typeof(ISpawner))]
    public class ReverseOrderSpawner : MonoBehaviour, ISpawnerPicker
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
                return -1;

            return index--;
        }
    }
}