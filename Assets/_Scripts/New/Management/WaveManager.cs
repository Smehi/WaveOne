﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<WaveConfigurator> waveConfigurators = new List<WaveConfigurator>();

        public List<WaveConfigurator> WaveConfigurators { get => waveConfigurators; }

        private void Start()
        {
            WaveConfigurator[] arr = FindObjectsOfType<WaveConfigurator>();
            for (int i = 0; i < arr.Length; i++)
            {
                waveConfigurators.Add(arr[i]);
            }
        }

        public void StartAllConfigWaves()
        {
            for (int i = 0; i < waveConfigurators.Count; i++)
            {
                waveConfigurators[i].SpawnerScript.StartWave();
            }
        }
    }
}