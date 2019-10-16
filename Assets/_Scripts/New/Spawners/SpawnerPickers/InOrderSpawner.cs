using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.Spawners.SpawnerPickers
{
    [RequireComponent(typeof(PerWaveCustom))]
    public class InOrderSpawner : MonoBehaviour, ISpawnerPicker
    {
        private List<int> list;
        private int index;

        public void SetListSize(int size)
        {
            list = new List<int>();
            index = 0;

            for (int i = 0; i < size; i++)
            {
                list.Add(i);
            }
        }

        public int GetIndex()
        {
            if (index == list.Count)
                return -1;

            return list[index++];
        }
    }
}