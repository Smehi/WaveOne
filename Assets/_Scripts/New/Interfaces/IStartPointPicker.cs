using System.Collections.Generic;

namespace WaveOne.StartPoints.StartPointPickers
{
    public interface IStartPointPicker<T>
    {
        void SetList(List<T> list);
        T GetListItem();
    }
}
