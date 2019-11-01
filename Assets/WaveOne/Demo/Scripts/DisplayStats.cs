using UnityEngine;
using UnityEngine.UI;

namespace SemihOrhan.WaveOne.Demo
{
#pragma warning disable 0649
    public class DisplayStats : MonoBehaviour
    {
        [SerializeField] private Text totalEnemiesText;
        [SerializeField] private Text deployedEnemiesText;
        [SerializeField] private Text AliveEnemiesText;

        private int totalEnemiesAmount;
        private int deployedEnemiesAmount;
        private int aliveEnemiesAmount;

        public void SetTotalEnemies(int amount)
        {
            totalEnemiesAmount += amount;
            totalEnemiesText.text = "Total Enemies: " + totalEnemiesAmount;
        }

        public void SetDeployedEnemies(int amount)
        {
            deployedEnemiesAmount += amount;
            deployedEnemiesText.text = "Deployed Enemies: " + deployedEnemiesAmount;
        }

        public void SetAliveEnemies(int amount)
        {
            aliveEnemiesAmount += amount;
            AliveEnemiesText.text = "Alive Enemies: " + aliveEnemiesAmount;
        }
    }
}