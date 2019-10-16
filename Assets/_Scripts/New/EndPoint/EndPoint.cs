using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.EndPoints
{
    public class EndPoint : MonoBehaviour
    {
        [SerializeField] private List<SinglePoint> endPoints = new List<SinglePoint>();
        [Tooltip("Put in all the enemies that should not have a end point. " +
                 "For example if you have your own logic for those enemies.")]
        [SerializeField] private List<GameObject> noEndPointEnemies = new List<GameObject>();

        private Dictionary<GameObject, List<Transform>> enemyEndPointsPairs = new Dictionary<GameObject, List<Transform>>();

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
