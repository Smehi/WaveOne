using System.Collections.Generic;
using UnityEngine;
using WaveOne.StartPoints.Shapes;
using WaveOne.StartPoints.StartPointPickers;

namespace WaveOne.StartPoints
{
#pragma warning disable 0649
    public class ListOfSpheres : MonoBehaviour, IStartPoint
    {
        [SerializeField] private List<Sphere> spawnPoints = new List<Sphere>();
        [SerializeField] private bool drawGizmos;

        private IStartPointPicker<Sphere> startPointPicker;
        private Vector3 v;

        private void Start()
        {
            startPointPicker = GetComponent<IStartPointPicker<Sphere>>();
            startPointPicker.SetList(spawnPoints);
        }

        [ContextMenu("Get a point")]
        public Vector3 GetPoint()
        {
            return GetRandomPointInSphere(startPointPicker.GetListItem());
        }

        private Vector3 GetRandomPointInSphere(Sphere sphere)
        {
            float x = Random.Range(0, 1f) * GetOneOrNegativeOne();
            float y = Random.Range(0, 1f) * GetOneOrNegativeOne();
            float z = Random.Range(0, 1f) * GetOneOrNegativeOne();
            Vector3 dirVector = new Vector3(x, y, z);

            Vector3 randomPoint = dirVector.normalized * Random.Range(sphere.minDistanceFromCenter, sphere.radius);
            Vector3 relativePoint = v = sphere.position + randomPoint;

            return relativePoint;
        }

        private int GetOneOrNegativeOne()
        {
            if (Random.Range(0f, 1f) < .5f)
                return -1;

            return 1;
        }

        #region Validation & Gizmos
        private void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Gizmos.DrawWireSphere(spawnPoints[i].position, spawnPoints[i].radius);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(v, .1f);
        }

        private void OnValidate()
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (spawnPoints[i].minDistanceFromCenter > spawnPoints[i].radius)
                {
                    spawnPoints[i] = new Sphere
                    {
                        position = spawnPoints[i].position,
                        radius = spawnPoints[i].radius,
                        minDistanceFromCenter = spawnPoints[i].radius
                    };
                }
                else if (spawnPoints[i].minDistanceFromCenter < 0)
                {
                    spawnPoints[i] = new Sphere
                    {
                        position = spawnPoints[i].position,
                        radius = spawnPoints[i].radius,
                        minDistanceFromCenter = 0
                    };
                }
            }
        }
        #endregion
    }
}