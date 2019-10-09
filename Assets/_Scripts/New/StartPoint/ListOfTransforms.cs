using System.Collections.Generic;
using UnityEngine;
using WaveOne.StartPoints.StartPointPickers;

namespace WaveOne.StartPoints
{
    public class ListOfTransforms : MonoBehaviour
    {
        [SerializeField] private SpawnPointPickerEnum.SpawnPointPickerType SpawnPointPickerType;
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
        [SerializeField] private bool drawGizmos;

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
            Transform listItem = startPointPicker.GetListItem();
            return v = listItem.position;
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(v, .1f);
        }
        #endregion
    }
}