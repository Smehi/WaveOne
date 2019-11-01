using System.Collections.Generic;
using SemihOrhan.WaveOne.StartPoints.Shapes;
using SemihOrhan.WaveOne.StartPoints.StartPointPickers;
using UnityEngine;

namespace SemihOrhan.WaveOne.StartPoints
{
#pragma warning disable 0649
    public class ListOfSpheres : MonoBehaviour, IStartPoint
    {
        [SerializeField] private List<Sphere> startPoints = new List<Sphere>();
        [SerializeField] private bool drawGizmos = true;

        private IStartPointPicker startPointPicker;
        private Vector3 v;

        public List<Sphere> StartPoints { get => startPoints; set => startPoints = value; }

        private void Start()
        {
            startPointPicker = GetComponent<IStartPointPicker>();
            startPointPicker.SetListSize(startPoints.Count);
        }

        public Vector3 GetPoint()
        {
            return GetRandomPointInSphere(startPoints[startPointPicker.GetIndex()]);
        }

        private Vector3 GetRandomPointInSphere(Sphere sphere)
        {
            float x = Random.Range(0, 1f) * GetOneOrNegativeOne();
            float y = Random.Range(0, 1f) * GetOneOrNegativeOne();
            float z = Random.Range(0, 1f) * GetOneOrNegativeOne();
            Vector3 dirVector = new Vector3(x, y, z);

            Vector3 randomPoint = dirVector.normalized * Random.Range(sphere.minDistanceFromCenter, sphere.radius);
            Vector3 relativePoint = v = sphere.transform.position + randomPoint;

            return relativePoint;
        }

        #region Helper functions
        /// <summary>
        /// Get a 1 or -1.
        /// </summary>
        /// <returns>1 or -1.</returns>
        private int GetOneOrNegativeOne()
        {
            if (Random.Range(0f, 1f) < .5f)
                return -1;

            return 1;
        }
        #endregion

        #region Validation & Gizmos
        private void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;

            for (int i = 0; i < startPoints.Count; i++)
            {
                if (startPoints[i].transform)
                    Gizmos.DrawWireSphere(startPoints[i].transform.position, startPoints[i].radius);
            }

            Gizmos.color = Color.black;
            for (int i = 0; i < startPoints.Count; i++)
            {
                if (startPoints[i].transform)
                    Gizmos.DrawSphere(startPoints[i].transform.position, startPoints[i].minDistanceFromCenter);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(v, .1f);
        }

        private void OnValidate()
        {
            for (int i = 0; i < startPoints.Count; i++)
            {
                if (startPoints[i].minDistanceFromCenter > startPoints[i].radius)
                {
                    startPoints[i] = new Sphere
                    {
                        transform = startPoints[i].transform,
                        radius = startPoints[i].radius,
                        minDistanceFromCenter = startPoints[i].radius
                    };
                }
                else if (startPoints[i].minDistanceFromCenter < 0)
                {
                    startPoints[i] = new Sphere
                    {
                        transform = startPoints[i].transform,
                        radius = startPoints[i].radius,
                        minDistanceFromCenter = 0
                    };
                }
            }
        }
        #endregion
    }
}