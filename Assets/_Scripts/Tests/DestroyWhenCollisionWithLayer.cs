using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class DestroyWhenCollisionWithLayer : MonoBehaviour
{
    [SerializeField] private int[] layersToCollideWith;

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
}
