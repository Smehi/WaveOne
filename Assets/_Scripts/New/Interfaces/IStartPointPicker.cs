using System.Collections.Generic;

namespace SemihOrhan.WaveOne.StartPoints.StartPointPickers
{
    public interface IStartPointPicker<T>
    {
        void SetList(List<T> list);
        T GetListItem();
    }
}
