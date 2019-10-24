using UnityEngine;
using UnityEngine.UI;

namespace SemihOrhan.WaveOne.Demo
{
#pragma warning disable 0649
    public class DisableButtonInWaves : MonoBehaviour
    {
        [SerializeField] private Button button;

        private WaveManager waveManager;
        int enemyCount;

        private void Start()
        {
            waveManager = GetComponent<WaveManager>();
        }

        public void SetButtonInteractable(bool val)
        {
            if (!val && !waveManager.SpawnersFinished)
                button.interactable = false;
        }

        public void CheckEnemyCount(int val)
        {
            enemyCount += val;

            if (enemyCount == 0 && waveManager.SpawnersFinished)
            {
                button.interactable = true;
            }
        }
    }
}
