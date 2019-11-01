using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.EndPoints
{
#pragma warning disable 0649
    public class EndPoint : MonoBehaviour
    {
#pragma warning disable 0414
        [Header("Editor controls")]
        [SerializeField] private bool showEndPointsListControls = true;
        [SerializeField] private bool showEnemiesListControls = true;
        [SerializeField] private bool showNoEndPointEnemiesListControls = true;
#pragma warning restore 0414

        [Header("End point settings")]
        [SerializeField] private List<SinglePoint> endPoints = new List<SinglePoint>();
        [SerializeField] private bool addColliders;
        [SerializeField] private bool triggerColliders;
        [SerializeField] private Vector3 colliderSize;
        [Tooltip("Put in all the enemies that should not have a end point. " +
                 "For example if you have your own logic for those enemies.")]
        [SerializeField] private List<GameObject> noEndPointEnemies = new List<GameObject>();

        private Dictionary<GameObject, List<Transform>> enemyEndPointsPairs = new Dictionary<GameObject, List<Transform>>();

        public List<SinglePoint> EndPoints { get => endPoints; set => endPoints = value; }
        public List<GameObject> NoEndPointEnemies { get => noEndPointEnemies; set => noEndPointEnemies = value; }

        private void Awake()
        {
            if (addColliders)
            {
                for (int i = 0; i < endPoints.Count; i++)
                {
                    BoxCollider col = endPoints[i].endPoint.gameObject.AddComponent<BoxCollider>();
                    col.size = colliderSize;
                    col.isTrigger = triggerColliders;
                }
            }
        }

        public void SetValidEndPointsPerEnemy(GameObject enemy)
        {
            if (noEndPointEnemies.Contains(enemy) || enemyEndPointsPairs.ContainsKey(enemy))
                return;

            List<Transform> availableEndPoints = new List<Transform>();

            for (int i = 0; i < endPoints.Count; i++)
            {
                if (endPoints[i].enemies.Contains(enemy) || endPoints[i].enemies.Count == 0)
                    availableEndPoints.Add(endPoints[i].endPoint);
            }

            enemyEndPointsPairs[enemy] = availableEndPoints;
        }

        public List<Transform> GetEndPoints(GameObject enemyPrefab)
        {
            if (noEndPointEnemies.Contains(enemyPrefab) || !enemyEndPointsPairs.ContainsKey(enemyPrefab))
                return null;

            return enemyEndPointsPairs[enemyPrefab];
        }

        #region Custom structs and validation
        [System.Serializable]
        public struct SinglePoint
        {
            [HideInInspector] public string name;
            public Transform endPoint;
            [Tooltip("Drag in all the enemies that are allowed to have this as their end point. " +
                     "Leave it empty if ALL enemies are allowed to have this as their end point.")]
            public List<GameObject> enemies;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < endPoints.Count; i++)
            {
                if (endPoints[i].endPoint)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(endPoints[i].endPoint.position, 0.1f);

                    if (addColliders)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(endPoints[i].endPoint.position, colliderSize);
                    } 
                }
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < endPoints.Count; i++)
            {
                endPoints[i] = new SinglePoint
                {
                    name = "End point " + (i + 1),
                    endPoint = endPoints[i].endPoint,
                    enemies = endPoints[i].enemies
                };
            }
        }
        #endregion
    }
}
