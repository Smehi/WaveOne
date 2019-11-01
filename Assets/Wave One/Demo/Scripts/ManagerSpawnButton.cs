using UnityEngine;
using UnityEngine.UI;

namespace SemihOrhan.WaveOne.Demo
{
#pragma warning disable 0649
    public class ManagerSpawnButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private WaveManager waveManager;

        private void Start()
        {
            waveManager = GetComponent<WaveManager>();
        }

        private void Update()
        {
            if (button.interactable == false && waveManager.SpawnersFinished && transform.childCount == 0)
                button.interactable = true;
        }

        public void SetButtonInteractable(bool val)
        {
            if (!val && !waveManager.SpawnersFinished && waveManager.SpawnersStarted)
                button.interactable = false;
        }
    }
}
