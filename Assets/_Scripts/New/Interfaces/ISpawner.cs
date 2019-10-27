using UnityEngine;

namespace SemihOrhan.WaveOne.Spawners
{
    public interface ISpawner
    {
        void StartWave();
        void StartWave(int wave);
        void SetEndPoint(GameObject prefabGameObject, GameObject instanciatedGameObject, int presetIndex);
        bool IsSpawnerDone();
        bool IsWaveCompleted(int wave);
    }
}
