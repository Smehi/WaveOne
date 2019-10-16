using System.Collections.Generic;
using UnityEngine;
using SemihOrhan.WaveOne.StartPoints.StartPointPickers;

namespace SemihOrhan.WaveOne.StartPoints
{
#pragma warning disable 0649
    public class ListOfTransforms : MonoBehaviour, IStartPoint
    {
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
        [SerializeField] private bool drawGizmos = true;

        private IStartPointPicker<Transform> startPointPicker;
        private Vector3 v;

        private void Start()
        {
            startPointPicker = GetComponent<IStartPointPicker<Transform>>();
            startPointPicker.SetList(spawnPoints);
        }

        [ContextMenu("Get a point")]
        public Vector3 GetPoint()
        {
            return v = startPointPicker.GetListItem().position;
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;

            Gizmos.color = Color.white;
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Gizmos.DrawWireSphere(spawnPoints[i].position, 0.2f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(v, .1f);
        }
        #endregion
    }
}