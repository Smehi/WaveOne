using UnityEngine;

namespace WaveOne.Spawners
{
    public interface ISpawner
    {
        void StartWave();
        void SetEndPoint(GameObject prefabGameObject, GameObject instanciatedGameObject, int presetIndex);
    }
}
