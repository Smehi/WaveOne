using System.Collections.Generic;
using UnityEngine;

namespace WaveOne.StartPoints.StartPointPickers
{
    public class InOrder<T> : MonoBehaviour, IStartPointPicker<T>
    {
        private List<T> list = new List<T>();
        private int index = 0;

        public void SetList(List<T> list)
        {
            this.list = list;
        }

        public T GetListItem()
        {
            if (index == list.Count)
                index = 0;

            return list[index++];
        }
    }
}