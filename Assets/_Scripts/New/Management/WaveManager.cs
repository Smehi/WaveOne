using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveOne
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<WaveConfigurator> waveConfigurators = new List<WaveConfigurator>();

        private void Start()
        {
            WaveConfigurator[] arr = FindObjectsOfType<WaveConfigurator>();
            for (int i = 0; i < arr.Length; i++)
            {
                waveConfigurators.Add(arr[i]);
            }
        }
    }
}