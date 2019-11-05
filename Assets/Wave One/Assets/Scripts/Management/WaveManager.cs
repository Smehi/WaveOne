using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private bool autoFindConfigs = true;
#pragma warning disable 0414
        [SerializeField] private bool showListControls = true;
#pragma warning restore 0414
        [SerializeField] private List<WaveConfigurator> waveConfigurators = new List<WaveConfigurator>();

        public List<WaveConfigurator> WaveConfigurators { get => waveConfigurators; set => waveConfigurators = value; }

        public bool SpawnersStarted { get; private set; }
        public int AmountSpawnersFinished { get; private set; }
        public bool SpawnersFinished { get; private set; }

        private void Start()
        {
            if (autoFindConfigs)
            {
                WaveConfigurator[] arr = FindObjectsOfType<WaveConfigurator>();
                for (int i = 0; i < arr.Length; i++)
                {
                    waveConfigurators.Add(arr[i]);
                }
            }
        }

        public void StartAllConfigWaves(int wave = -1)
        {
            SpawnersStarted = true;
            AmountSpawnersFinished = 0;
            SpawnersFinished = false;

            for (int i = 0; i < waveConfigurators.Count; i++)
            {
                if (wave != -1)
                {
                    waveConfigurators[i].SpawnerScript.StartWave(wave);
                    continue;
                }
                waveConfigurators[i].SpawnerScript.StartWave();
            }
        }

        public void SpawnerFinished(bool val)
        {
            if (val && SpawnersStarted)
            {
                for (int i = 0; i < waveConfigurators.Count; i++)
                {
                    if (waveConfigurators[i].SpawnerScript.IsSpawnerDone())
                        AmountSpawnersFinished++;
                }

                if (AmountSpawnersFinished == WaveConfigurators.Count)
                    SpawnersFinished = true;
            }
        }
    }
}