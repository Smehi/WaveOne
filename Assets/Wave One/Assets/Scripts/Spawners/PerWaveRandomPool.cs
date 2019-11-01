using System.Collections;
using System.Collections.Generic;
using SemihOrhan.WaveOne.EndPoints;
using SemihOrhan.WaveOne.Events;
using UnityEngine;

namespace SemihOrhan.WaveOne.Spawners
{
#pragma warning disable 0649
    public class PerWaveRandomPool : MonoBehaviour, ISpawner
    {
#pragma warning disable 0414
        [Header("Editor controls")]
        [SerializeField] private bool showWaveListControls = true;
        [SerializeField] private bool showEnemiesListControls = true;
#pragma warning restore 0414

        [Header("Wave settings")]
        [SerializeField] private List<SingleWave> enemyWaves = new List<SingleWave>();
        [SerializeField] private float minTimeForNextDeployment;
        [SerializeField] private float maxTimeForNextDeployment;
        [Tooltip("Enemies per second")]
        [SerializeField] private float spawnRate;
        [Tooltip("Automatically invoke the next deployment")]
        [SerializeField] private bool autoDeploy = true;

        [Header("Miscellaneous")]
        [Tooltip("Name of the parent GameObject to spawn enemies in. To indicate a child of another object use a \"/\"." +
                 "Keep empty if you don't want to spawn in another GameObject.")]
        [SerializeField] private string enemyParentObject;
        [SerializeField] private BoolEvent eventSpawnerFinished;
        [SerializeField] private IntEvent eventTotalEnemies;
        [SerializeField] private IntEvent eventDeployedEnemies;
        [SerializeField] private IntEvent eventAliveEnemies;

        private int currentWave; // 0 indexed.
        private int currentDeployment; // 1 indexed.
        private bool waveInProgress;

        private WaveConfigurator waveConfig;
        private EndPoint endPoints;
        private IEnumerator currentIEnumerator;
        private List<bool> waveCompletion = new List<bool>();

        public List<SingleWave> EnemyWaves { get => enemyWaves; set => enemyWaves = value; }
        public float MinTimeForNextDeployment { get => minTimeForNextDeployment; set => minTimeForNextDeployment = value; }
        public float MaxTimeForNextDeployment { get => maxTimeForNextDeployment; set => maxTimeForNextDeployment = value; }
        public float SpawnRate { get => spawnRate; set => spawnRate = value; }
        public bool AutoDeploy { get => autoDeploy; set => autoDeploy = value; }
        public Transform Parent { get; set; }
        public bool SetEndPoints { get; set; }

        private void Start()
        {
            // Cache the parent GameObject.
            GameObject go = GameObject.Find(enemyParentObject);
            Parent = go != null ? go.transform : null;

            // Cache the spawnRate
            spawnRate = 1 / spawnRate;

            currentWave = 0;
            currentDeployment = 1;

            for (int i = 0; i < enemyWaves.Count; i++)
                waveCompletion.Add(false);

            endPoints = GetComponent<EndPoint>();

            if (endPoints)
            {
                SetEndPoints = true;

                for (int i = 0; i < enemyWaves.Count; i++)
                {
                    if (eventTotalEnemies != null)
                        eventTotalEnemies.Raise(enemyWaves[i].poolSize);

                    for (int j = 0; j < enemyWaves[i].enemies.Count; j++)
                        endPoints.SetValidEndPointsPerEnemy(enemyWaves[i].enemies[j].gameObject);
                }
            }

            waveConfig = GetComponent<WaveConfigurator>();
        }

        public void StartWave()
        {
            if (currentWave >= enemyWaves.Count)
            {
                Debug.Log("Final wave already reached. No more waves left!");
                return;
            }

            if (currentIEnumerator != null)
                StopCoroutine(currentIEnumerator);

            currentIEnumerator = DeployTroops(currentWave);
            StartCoroutine(currentIEnumerator);

            if (eventSpawnerFinished != null && !waveInProgress)
            {
                eventSpawnerFinished.Raise(false);
                waveInProgress = true;
            }
        }

        public void StartWave(int wave)
        {
            if (wave < 0 || wave >= enemyWaves.Count)
            {
                Debug.LogError("Given wave number is invalid");
                return;
            }

            if (currentIEnumerator != null)
                StopCoroutine(currentIEnumerator);

            currentIEnumerator = DeployTroops(wave);
            StartCoroutine(currentIEnumerator);

            if (eventSpawnerFinished != null && !waveInProgress)
            {
                eventSpawnerFinished.Raise(false);
                waveInProgress = true;
            }
        }

        private IEnumerator DeployTroops(int currentWave)
        {
            int deployedCount = 0;

            // Divide amount of enemies by the amount of deployments to get the amount to deploy.
            int toDeploy = Mathf.FloorToInt(enemyWaves[currentWave].poolSize / enemyWaves[currentWave].deployments);

            // This means we have reached the last deployment.
            // At the last deployment we want to add the remainder of the enemies count.
            // eg. 17 % 3 = 2. This gives us a normal deployment of 5 and the remaining 2 for the last deployment.
            if (enemyWaves[currentWave].deployments != 1 && currentDeployment == enemyWaves[currentWave].deployments)
                toDeploy += enemyWaves[currentWave].poolSize % enemyWaves[currentWave].deployments;

            while (deployedCount < toDeploy)
            {
                int leftOverPoolSize = toDeploy - deployedCount;

                if (leftOverPoolSize <= 0)
                    yield return null;

                int enemyIndex = Random.Range(0, enemyWaves[currentWave].enemies.Count);

                bool gotRelativeGroupPositions = false;
                List<Vector3> relativeGroupPositions = new List<Vector3>();
                Vector3 spawnPointPos = waveConfig.StartPointScript.GetPoint();
                int presetIndexEndPoint = Random.Range(0, endPoints.GetEndPoints(enemyWaves[currentWave].enemies[enemyIndex].gameObject).Count);
                int spawnAmount = enemyWaves[currentWave].enemies[enemyIndex].groupSize;

                // There aren't enough spots left in the pool to accommodate for the whole groupsize
                // of this enemy so we have to lower the amount we want to spawn.
                if (leftOverPoolSize < enemyWaves[currentWave].enemies[enemyIndex].groupSize)
                    spawnAmount = leftOverPoolSize;

                for (int i = 0; i < spawnAmount; i++)
                {
                    GameObject instance;
                    if (Parent)
                        instance = Instantiate(enemyWaves[currentWave].enemies[enemyIndex].gameObject,
                                               spawnPointPos,
                                               Quaternion.identity,
                                               Parent);
                    else
                        instance = Instantiate(enemyWaves[currentWave].enemies[enemyIndex].gameObject,
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

                    deployedCount++;

                    if (SetEndPoints)
                    {
                        SetEndPoint(enemyWaves[currentWave].enemies[enemyIndex].gameObject,
                                    instance,
                                    presetIndexEndPoint);
                    }
                }

                yield return new WaitForSeconds(spawnRate);
            }

            // If this is the last deployment we want to reset the deployments and increase the wave counter.
            if (currentDeployment == enemyWaves[currentWave].deployments)
            {
                this.currentWave++;
                currentDeployment = 1;
                waveInProgress = false;
                waveCompletion[currentWave] = true;

                if (eventSpawnerFinished != null)
                    eventSpawnerFinished.Raise(true);
            }
            else
            {
                currentDeployment++;

                if (autoDeploy)
                {
                    float randomTime = Random.Range(minTimeForNextDeployment, maxTimeForNextDeployment);
                    Invoke("StartWave", randomTime);
                }
            }
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
            return waveCompletion[wave];
        }

        #region Custom object structs
        [System.Serializable]
        public struct SingleWave
        {
            [HideInInspector] public string name;
            public List<EnemyCount> enemies;
            public int poolSize;
            public int deployments;
        }

        [System.Serializable]
        public struct EnemyCount
        {
            [HideInInspector] public string name;
            public GameObject gameObject;
            public int groupSize;
        }
        #endregion

        #region Variable validation
        private void OnValidate()
        {
            if (spawnRate <= 0)
                spawnRate = 0.1f;

            if (minTimeForNextDeployment < 0)
                minTimeForNextDeployment = 0;

            if (minTimeForNextDeployment > maxTimeForNextDeployment)
                minTimeForNextDeployment = maxTimeForNextDeployment;

            if (maxTimeForNextDeployment < minTimeForNextDeployment)
                maxTimeForNextDeployment = minTimeForNextDeployment;

            for (int i = 0; i < enemyWaves.Count; i++)
            {
                for (int j = 0; j < enemyWaves[i].enemies.Count; j++)
                {
                    enemyWaves[i].enemies[j] = new EnemyCount
                    {
                        name = "Enemy " + (j + 1),
                        gameObject = enemyWaves[i].enemies[j].gameObject,
                        groupSize = enemyWaves[i].enemies[j].groupSize
                    };

                    if (enemyWaves[i].enemies[j].groupSize < 1)
                        enemyWaves[i].enemies[j] = new EnemyCount
                        {
                            name = enemyWaves[i].enemies[j].name,
                            gameObject = enemyWaves[i].enemies[j].gameObject,
                            groupSize = 1
                        };
                    else if (enemyWaves[i].enemies[j].groupSize > enemyWaves[i].poolSize)
                        enemyWaves[i].enemies[j] = new EnemyCount
                        {
                            name = enemyWaves[i].enemies[j].name,
                            gameObject = enemyWaves[i].enemies[j].gameObject,
                            groupSize = enemyWaves[i].poolSize
                        };
                }

                enemyWaves[i] = new SingleWave
                {
                    name = "Wave " + (i + 1),
                    enemies = enemyWaves[i].enemies,
                    poolSize = enemyWaves[i].poolSize,
                    deployments = enemyWaves[i].deployments
                };

                if (enemyWaves[i].poolSize < 1)
                    enemyWaves[i] = new SingleWave
                    {
                        name = enemyWaves[i].name,
                        enemies = enemyWaves[i].enemies,
                        poolSize = 1,
                        deployments = enemyWaves[i].deployments
                    };

                if (enemyWaves[i].deployments < 1)
                    enemyWaves[i] = new SingleWave
                    {
                        name = enemyWaves[i].name,
                        enemies = enemyWaves[i].enemies,
                        poolSize = enemyWaves[i].poolSize,
                        deployments = 1
                    };
            }
        }
        #endregion
    }
}
