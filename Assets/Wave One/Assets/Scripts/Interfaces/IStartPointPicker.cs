using System.Collections.Generic;

namespace SemihOrhan.WaveOne.StartPoints.StartPointPickers
{
    public interface IStartPointPicker
    {
        void SetListSize(int size);
        int GetIndex();
    }
}
