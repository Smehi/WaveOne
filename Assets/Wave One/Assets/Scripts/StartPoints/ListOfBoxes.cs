using SemihOrhan.WaveOne.StartPoints.Shapes;
using SemihOrhan.WaveOne.StartPoints.StartPointPickers;
using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.StartPoints
{
#pragma warning disable 0649
    public class ListOfBoxes : MonoBehaviour, IStartPoint
    {
        [SerializeField] private List<Box> startPoints = new List<Box>();
        [Tooltip("This setting is mostly important if your minimun spawn box is the same size as the full spawn box. " +
                 "Setting it to false will mean all axi are maxed resulting in a corner spawn. " +
                 "Setting it to true will mean only 1 axis is maxed resulting in a random spawn on a certain face of the box.")]
        [SerializeField] private bool setOneAxisToMinimum = true;
        [SerializeField] private bool drawGizmos = true;

        private IStartPointPicker startPointPicker;
        private Vector3 v;

        public List<Box> StartPoints { get => startPoints; set => startPoints = value; }

        private void Start()
        {
            startPointPicker = GetComponent<IStartPointPicker>();
            startPointPicker.SetListSize(startPoints.Count);
        }

        public Vector3 GetPoint()
        {
            return GetRandomPointInBox(startPoints[startPointPicker.GetIndex()]);
        }

        private Vector3 GetRandomPointInBox(Box box)
        {
            // Get a reference to the half so we don't need to divide often.
            Vector3 boxHalf = box.size / 2f;
            Vector3 boxMinDistHalf = box.minDistanceFromCenter / 2f;

            // Size is the whole length of a side, so we only want half of each axis.
            // We can then randomly invert each so we can reach each octant of the box.
            float x = Random.Range(0, boxHalf.x) * GetOneOrNegativeOne();
            float y = Random.Range(0, boxHalf.y) * GetOneOrNegativeOne();
            float z = Random.Range(0, boxHalf.z) * GetOneOrNegativeOne();

            if (setOneAxisToMinimum)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        x = GetMinUpToMax(x, boxMinDistHalf.x, boxHalf.x);
                        break;
                    case 1:
                        y = GetMinUpToMax(y, boxMinDistHalf.y, boxHalf.y);
                        break;
                    case 2:
                        z = GetMinUpToMax(z, boxMinDistHalf.z, boxHalf.z);
                        break;
                }
            }
            else
            {
                x = GetMinUpToMax(x, boxMinDistHalf.x, boxHalf.x);
                y = GetMinUpToMax(y, boxMinDistHalf.y, boxHalf.y);
                z = GetMinUpToMax(z, boxMinDistHalf.z, boxHalf.z);
            }

            // Add the Vector3 we got to the box base position because the box isn't always at (0, 0, 0).
            return v = (box.transform.position + new Vector3(x, y, z));
        }

        #region Helper functions
        /// <summary>
        /// Sets the input value to the minimum value and up to the max value.
        /// </summary>
        /// <param name="f1">Input value</param>
        /// <param name="f2">Minimum value</param>
        /// <param name="f3">Maximum value</param>
        /// <returns>value that is at least f2 at most f3 or inbetween.</returns>
        private float GetMinUpToMax(float f1, float f2, float f3)
        {
            // Check if our float variable against our min float.
            // If it's bigger we don't need to do anything otherwise we need a new value.
            if (Mathf.Abs(f1) < Mathf.Abs(f2))
            {
                // Return the minimum value depening on the state of the original value
                // (negative/positive). Then add a random value between the max and min.
                if (f1 < 0)
                    return -f2 - Random.Range(0f, f3 - f2);
                else
                    return f2 + Random.Range(0f, f3 - f2);
            }

            return f1;
        }

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
                    Gizmos.DrawWireCube(startPoints[i].transform.position, startPoints[i].size);
            }

            Gizmos.color = Color.black;
            for (int i = 0; i < startPoints.Count; i++)
            {
                if (startPoints[i].transform)
                    Gizmos.DrawCube(startPoints[i].transform.position, startPoints[i].minDistanceFromCenter);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(v, .1f);
        }

        /// <summary>
        /// Set the vector value to another vector if conditions meet.
        /// </summary>
        /// <param name="v1">Input vector</param>
        /// <param name="v2">Vector to check the input against</param>
        /// <param name="checkIfBigger">The condition to check</param>
        /// <returns>v1 with xyz capped off at v2 if conditions meet.</returns>
        private Vector3 SetMinMaxVector(Vector3 v1, Vector3 v2, bool checkIfBigger)
        {
            if (checkIfBigger)
            {
                if (v1.x > v2.x)
                {
                    v1.x = v2.x;
                }

                if (v1.y > v2.y)
                {
                    v1.y = v2.y;
                }

                if (v1.z > v2.z)
                {
                    v1.z = v2.z;
                }
            }
            else
            {
                if (v1.x < v2.x)
                {
                    v1.x = v2.x;
                }

                if (v1.y < v2.y)
                {
                    v1.y = v2.y;
                }

                if (v1.z < v2.z)
                {
                    v1.z = v2.z;
                }
            }

            return v1;
        }

        private void OnValidate()
        {
            for (int i = 0; i < startPoints.Count; i++)
            {
                // If size values are smaller than 0.
                if (startPoints[i].size.x < 0 ||
                    startPoints[i].size.y < 0 ||
                    startPoints[i].size.z < 0)
                {
                    startPoints[i] = new Box
                    {
                        transform = startPoints[i].transform,
                        size = SetMinMaxVector(startPoints[i].size, Vector3.zero, false),
                        minDistanceFromCenter = startPoints[i].minDistanceFromCenter
                    };
                }

                // If minDistance values are smaller than 0.
                if (startPoints[i].minDistanceFromCenter.x < 0 ||
                    startPoints[i].minDistanceFromCenter.y < 0 ||
                    startPoints[i].minDistanceFromCenter.z < 0)
                {
                    startPoints[i] = new Box
                    {
                        transform = startPoints[i].transform,
                        size = startPoints[i].size,
                        minDistanceFromCenter = SetMinMaxVector(startPoints[i].minDistanceFromCenter, Vector3.zero, false)
                    };
                }

                // If minDistance values are bigger than the size.
                if (startPoints[i].minDistanceFromCenter.x > startPoints[i].size.x ||
                    startPoints[i].minDistanceFromCenter.y > startPoints[i].size.y ||
                    startPoints[i].minDistanceFromCenter.z > startPoints[i].size.z)
                {
                    startPoints[i] = new Box
                    {
                        transform = startPoints[i].transform,
                        size = startPoints[i].size,
                        minDistanceFromCenter = SetMinMaxVector(startPoints[i].minDistanceFromCenter, startPoints[i].size, true)
                    };
                }
            }
        }
        #endregion
    }
}