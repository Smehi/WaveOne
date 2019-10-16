﻿using UnityEngine;
using UnityEngine.UI;

namespace SemihOrhan.WaveOne.Demo
{
    public class DisableButtonInWaves : MonoBehaviour
    {
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
        }

        public void SetButtonInteractable(bool state)
        {
            button.interactable = !state;
        }
    } 
}
