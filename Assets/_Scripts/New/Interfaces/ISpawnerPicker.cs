using System.Collections.Generic;

namespace SemihOrhan.WaveOne.Spawners.SpawnerPickers
{
    public interface ISpawnerPicker
    {
        void SetListSize(int size);
        int GetIndex();
    }
}
