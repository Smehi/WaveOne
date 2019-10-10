using System.Collections.Generic;
using UnityEngine;

namespace WaveOne.StartPoints.StartPointPickers
{
    public class ReverseOrder<T> : MonoBehaviour, IStartPointPicker<T>
    {
        private List<T> list = new List<T>();
        private int index = 0;

        public void SetList(List<T> list)
        {
            this.list = list;
            index = list.Count - 1;
        }

        public T GetListItem()
        {
            if (index == -1)
                index = list.Count - 1;

            return list[index--];
        }
    }
}