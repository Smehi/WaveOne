using SemihOrhan.WaveOne.Events;
using UnityEngine;

namespace SemihOrhan.WaveOne.Demo
{
#pragma warning disable 0649
    public class DestroyWhenCollisionWithLayer : MonoBehaviour
    {
        [SerializeField] private int[] layersToCollideWith;
        [SerializeField] private IntEvent eventAliveEnemies;

        private void OnTriggerEnter(Collider other)
        {
            bool noCollision = true;

            for (int i = 0; i < layersToCollideWith.Length; i++)
            {
                if (other.gameObject.layer == layersToCollideWith[i])
                    noCollision = false;
            }

            if (noCollision)
                return;

            Destroy(gameObject);
        }

        private void OnDisable()
        {
            if (eventAliveEnemies != null)
                eventAliveEnemies.Raise(-1);
        }
    }
}