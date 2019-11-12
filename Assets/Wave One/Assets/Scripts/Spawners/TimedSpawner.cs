using System.Collections;
using System.Collections.Generic;
using SemihOrhan.WaveOne.EndPoints;
using SemihOrhan.WaveOne.Events;
using UnityEngine;

namespace SemihOrhan.WaveOne.Spawners
{
#pragma warning disable 0649
    public class TimedSpawner : MonoBehaviour, ISpawner
    {
#pragma warning disable 0414
        [Header("Editor controls")]
        [SerializeField] private bool showEnemiesListControls = true;
#pragma warning restore 0414

        [Header("Spawner settings")]
        [SerializeField] private List<EnemyWithWeight> enemyList = new List<EnemyWithWeight>();
        [Tooltip("Length in seconds, leave 0 if endless")]
        [SerializeField] float maxTime;
        [Tooltip("Enemies per second")]
        [SerializeField] private float spawnRate;

        [Header("Miscellaneous")]
        [Tooltip("Name of the parent GameObject to spawn enemies in. To indicate a child of another object use a \"/\"." +
                 "Keep empty if you don't want to spawn in another GameObject.")]
        [SerializeField] private string enemyParentObject;
        [SerializeField] private BoolEvent eventSpawnerFinished;
        [SerializeField] private IntEvent eventDeployedEnemies;
        [SerializeField] private IntEvent eventAliveEnemies;

        private bool waveInProgress;
        private bool isEndless;

        private WaveConfigurator waveConfig;
        private EndPoint endPoints;
        private IEnumerator currentIEnumerator;

        public List<EnemyWithWeight> EnemyList { get => enemyList; set => enemyList = value; }
        public float MaxTime { get => maxTime; set => maxTime = value; }
        public float SpawnRate { get => spawnRate; set => spawnRate = value; }
        public Transform Parent { get; set; }
        public bool SetEndPoints { get; set; }

        private void Start()
        {
            // Cache the parent GameObject.
            GameObject go = GameObject.Find(enemyParentObject);
            Parent = go != null ? go.transform : null;

            // Cache the spawnRate
            spawnRate = 1 / spawnRate;
            isEndless = maxTime == 0;

            endPoints = GetComponent<EndPoint>();

            if (endPoints)
            {
                SetEndPoints = true;

                for (int i = 0; i < enemyList.Count; i++)
                {
                    endPoints.SetValidEndPointsPerEnemy(enemyList[i].gameObject);
                }
            }

            waveConfig = GetComponent<WaveConfigurator>();
        }

        public void StartWave()
        {
            if (currentIEnumerator != null)
                StopCoroutine(currentIEnumerator);

            currentIEnumerator = DeployTroops();
            StartCoroutine(currentIEnumerator);

            if (eventSpawnerFinished != null && !waveInProgress)
            {
                eventSpawnerFinished.Raise(false);
                waveInProgress = true;
            }
        }

        // No waves in timed spawner but we still need to start the wave.
        public void StartWave(int wave)
        {
            Debug.Log("TimedSpawner does not have waves. Spawning like normal.");

            if (!waveInProgress)
                StartWave();
        }

        private void Update()
        {
            if (!isEndless && waveInProgress && maxTime > 0)
                maxTime -= Time.deltaTime;
        }

        private IEnumerator DeployTroops()
        {
            if (isEndless)
                maxTime = 1;

            while (maxTime > 0)
            {
                int enemyIndex = GetEnemyIndex();

                bool gotRelativeGroupPositions = false;
                List<Vector3> relativeGroupPositions = new List<Vector3>();
                Vector3 spawnPointPos = waveConfig.StartPointScript.GetPoint();
                int presetIndexEndPoint = Random.Range(0, endPoints.GetEndPoints(enemyList[enemyIndex].gameObject).Count);
                int spawnAmount = enemyList[enemyIndex].groupSize;

                for (int i = 0; i < spawnAmount; i++)
                {
                    GameObject instance;
                    if (Parent)
                        instance = Instantiate(enemyList[enemyIndex].gameObject,
                                               spawnPointPos,
                                               Quaternion.identity,
                                               Parent);
                    else
                        instance = Instantiate(enemyList[enemyIndex].gameObject,
                                               spawnPointPos,
                                               Quaternion.identity);

                    // We can only get the bounds of the collider of an active object.
                    // This means that we can't use the Prefab to get the info we need.
                    // So we first spawn the object and then from that instance we get the info we need,
                    // then we apply the relative position at the end.
                    if (!gotRelativeGroupPositions)
                    {
                        relativeGroupPositions = waveConfig.FormationScript.MakeFormation(instance,
                                                                                          spawnAmount);
                        gotRelativeGroupPositions = true;
                    }

                    instance.transform.position += relativeGroupPositions[i];

                    if (eventDeployedEnemies != null)
                        eventDeployedEnemies.Raise(1);

                    if (eventAliveEnemies != null)
                        eventAliveEnemies.Raise(1);

                    if (SetEndPoints)
                    {
                        SetEndPoint(enemyList[enemyIndex].gameObject,
                                    instance,
                                    presetIndexEndPoint);
                    }
                }

                yield return new WaitForSeconds(spawnRate);
            }

            waveInProgress = false;

            if (eventSpawnerFinished != null)
                eventSpawnerFinished.Raise(true);
        }

        public void SetEndPoint(GameObject prefabGameObject, GameObject instanciatedGameObject, int presetIndex)
        {
            instanciatedGameObject.AddComponent(typeof(SetAgentDestination));
            SetAgentDestination sad = instanciatedGameObject.GetComponent<SetAgentDestination>();
            List<Transform> result = endPoints.GetEndPoints(prefabGameObject);

            if (result != null)
                sad.CalculateValidPath(result, presetIndex);
        }

        public bool IsSpawnerDone()
        {
            if (waveInProgress)
                return false;

            return true;
        }

        public bool IsWaveCompleted(int wave)
        {
            return IsSpawnerDone();
        }

        // Getting a random enemy using the weights that were given.
        private int GetEnemyIndex()
        {
            int index = -1;
            int total = 0;

            for (int i = 0; i < enemyList.Count; i++)
                total += enemyList[i].weight;

            float r = Random.value * total;
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (r < enemyList[i].weight)
                {
                    index = i;
                    break;
                }

                r -= enemyList[i].weight;
            }

            return index;
        }

        #region Custom struct and validation
        [System.Serializable]
        public struct EnemyWithWeight
        {
            [HideInInspector] public string name;
            public GameObject gameObject;
            public int groupSize;
            public int weight;
        }

        private void OnValidate()
        {
            if (maxTime < 0)
                maxTime = 0;

            if (spawnRate < 0)
                spawnRate = 0.1f;

            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i] = new EnemyWithWeight
                {
                    name = "Enemy " + (i + 1),
                    gameObject = enemyList[i].gameObject,
                    groupSize = enemyList[i].groupSize,
                    weight = enemyList[i].weight
                };

                if (enemyList[i].groupSize < 1)
                {
                    enemyList[i] = new EnemyWithWeight
                    {
                        name = enemyList[i].name,
                        gameObject = enemyList[i].gameObject,
                        groupSize = 1,
                        weight = enemyList[i].weight
                    };
                }

                if (enemyList[i].weight < 0)
                {
                    enemyList[i] = new EnemyWithWeight
                    {
                        name = enemyList[i].name,
                        gameObject = enemyList[i].gameObject,
                        groupSize = enemyList[i].groupSize,
                        weight = 0
                    };
                }
            }
        }
    }
    #endregion
}