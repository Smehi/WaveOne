using System.Collections.Generic;
using UnityEngine;
using WaveOne.StartPoints.Shapes;
using WaveOne.StartPoints.StartPointPickers;

namespace WaveOne.StartPoints
{
#pragma warning disable 0649
    public class ListOfBoxes : MonoBehaviour, IStartPoint
    {
        [SerializeField] private List<Box> spawnPoints = new List<Box>();
        [SerializeField] private bool drawGizmos;

        private IStartPointPicker<Box> startPointPicker;
        private Vector3 v;

        private void Start()
        {
            startPointPicker = GetComponent<IStartPointPicker<Box>>();
            startPointPicker.SetList(spawnPoints);
        }

        [ContextMenu("Get a point")]
        public Vector3 GetPoint()
        {
            return GetRandomPointInBox(startPointPicker.GetListItem());
        }

        private Vector3 GetRandomPointInBox(Box box)
        {
            // Size is the whole length of a side, but we only want half of each axis
            // so we get a octant of the box. We can then randomly invert each so 
            // we can reach each octant.
            float x = Random.Range(box.minDistanceFromCenter.x / 2f, box.size.x / 2f) * GetOneOrNegativeOne();
            float y = Random.Range(box.minDistanceFromCenter.y / 2f, box.size.y / 2f) * GetOneOrNegativeOne();
            float z = Random.Range(box.minDistanceFromCenter.z / 2f, box.size.z / 2f) * GetOneOrNegativeOne();

            // Add the Vector3 we got to the box base position because the box isn't always at (0, 0, 0).
            return v = (box.position + new Vector3(x, y, z));
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
                Gizmos.DrawWireCube(spawnPoints[i].position, spawnPoints[i].size);
            }

            Gizmos.color = Color.black;
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Gizmos.DrawCube(spawnPoints[i].position, spawnPoints[i].minDistanceFromCenter);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(v, .1f);
        }

        private Vector3 SetMinMax(Vector3 inputVector, Vector3 checkAgainstVector, bool checkIfBigger)
        {
            if (checkIfBigger)
            {
                if (inputVector.x > checkAgainstVector.x)
                {
                    inputVector.x = checkAgainstVector.x;
                }

                if (inputVector.y > checkAgainstVector.y)
                {
                    inputVector.y = checkAgainstVector.y;
                }

                if (inputVector.z > checkAgainstVector.z)
                {
                    inputVector.z = checkAgainstVector.z;
                } 
            }
            else
            {
                if (inputVector.x < checkAgainstVector.x)
                {
                    inputVector.x = checkAgainstVector.x;
                }

                if (inputVector.y < checkAgainstVector.y)
                {
                    inputVector.y = checkAgainstVector.y;
                }

                if (inputVector.z < checkAgainstVector.z)
                {
                    inputVector.z = checkAgainstVector.z;
                }
            }

            return inputVector;
        }

        private void OnValidate()
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                // If size values are smaller than 0.
                if (spawnPoints[i].size.x < 0 ||
                    spawnPoints[i].size.y < 0 ||
                    spawnPoints[i].size.z < 0)
                {
                    spawnPoints[i] = new Box
                    {
                        position = spawnPoints[i].position,
                        size = SetMinMax(spawnPoints[i].size, Vector3.zero, false),
                        minDistanceFromCenter = spawnPoints[i].minDistanceFromCenter
                    };
                }

                // If minDistance values are smaller than 0.
                if (spawnPoints[i].minDistanceFromCenter.x < 0 ||
                    spawnPoints[i].minDistanceFromCenter.y < 0 ||
                    spawnPoints[i].minDistanceFromCenter.z < 0)
                {
                    spawnPoints[i] = new Box
                    {
                        position = spawnPoints[i].position,
                        size = spawnPoints[i].size,
                        minDistanceFromCenter = SetMinMax(spawnPoints[i].minDistanceFromCenter, Vector3.zero, false)
                    };
                }

                // If minDistance values are bigger than the size.
                if (spawnPoints[i].minDistanceFromCenter.x > spawnPoints[i].size.x ||
                    spawnPoints[i].minDistanceFromCenter.y > spawnPoints[i].size.y ||
                    spawnPoints[i].minDistanceFromCenter.z > spawnPoints[i].size.z)
                {
                    spawnPoints[i] = new Box
                    {
                        position = spawnPoints[i].position,
                        size = spawnPoints[i].size,
                        minDistanceFromCenter = SetMinMax(spawnPoints[i].minDistanceFromCenter, spawnPoints[i].size, true)
                    };
                }
            }
        }
        #endregion
    }
}