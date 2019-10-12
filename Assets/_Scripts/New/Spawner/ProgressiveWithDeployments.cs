using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveOne.EndPoints;
using WaveOne.Events;

namespace WaveOne.Spawners
{
#pragma warning disable 0649
    public class ProgressiveWithDeployments : MonoBehaviour, ISpawner
    {
        [Header("Waves")]
        [SerializeField] private List<SingleWave> enemyWaves = new List<SingleWave>();
        [SerializeField] private float minTimeForNextDeployment;
        [SerializeField] private float maxTimeForNextDeployment;
        [SerializeField, Tooltip("Enemies per second")] private float spawnRate;

        [Header("Miscellaneous")]
        [Tooltip("Name of the parent GameObject to spawn enemies in. To indicate a child of another object use a \"/\"." +
                 "Keep empty if you don't want to spawn in another GameObject.")]
        [SerializeField] private string enemyParentObject;
        [SerializeField] private BoolEvent eventWaveInProgress;

        private int currentWave; // 0 indexed.
        private int currentDeployment; // 1 indexed.
        private Transform parent;
        private bool setEndPoints;

        private WaveConfigurator waveConfig;
        private EndPoint endPoints;

        private void Start()
        {
            // Cache the parent GameObject.
            GameObject go = GameObject.Find(enemyParentObject);
            parent = go != null ? go.transform : null;

            // Cache the spawnRate
            spawnRate = 1 / spawnRate;

            currentWave = 0;
            currentDeployment = 1;

            endPoints = GetComponent<EndPoint>();

            if (endPoints)
            {
                setEndPoints = true;

                for (int i = 0; i < enemyWaves.Count; i++)
                    for (int j = 0; j < enemyWaves[i].enemies.Count; j++)
                        endPoints.SetValidEndPointsPerEnemy(enemyWaves[i].enemies[j].gameObject);
            }

            waveConfig = GetComponent<WaveConfigurator>();
        }

        [ContextMenu("Start wave")]
        public void StartWave()
        {
            StartCoroutine(DeployTroops(currentWave));

            if (eventWaveInProgress != null)
                eventWaveInProgress.Raise(true);
        }

        private IEnumerator DeployTroops(int currentWave)
        {
            bool finishedDeploying = false;
            bool calculatedTroops = false;
            int toDeploy = 0;
            int deployedCount = 0;
            int currentEnemy = 0;

            // The coroutine only runs for a single deployment.
            while (!finishedDeploying)
            {
                // Calculate the amount of troops to deploy for the current deployment and current enemy.
                if (!calculatedTroops)
                {
                    if (currentEnemy < enemyWaves[currentWave].enemies.Count)
                    {
                        toDeploy = Mathf.FloorToInt(enemyWaves[currentWave].enemies[currentEnemy].amount / enemyWaves[currentWave].deployments);

                        // This means we have reached the last deployment.
                        // At the last deployment we want to add the remainder of the enemies count.
                        // eg. 17 % 3 = 2. This gives us a normal deployment of 5 and the remaining 2 for the last deployment.
                        if (enemyWaves[currentWave].deployments != 1 && currentDeployment == enemyWaves[currentWave].deployments)
                        {
                            toDeploy += enemyWaves[currentWave].enemies[currentEnemy].amount % enemyWaves[currentWave].deployments;
                        }
                    }
                    else
                    {
                        // This means we have had all the enemies and we finished the current deployment.
                        finishedDeploying = true;
                    }

                    // We only want to recalculate the amount to deploy when we are done with the current enemy
                    // so we trigger this flag for the time being.
                    calculatedTroops = true;
                }

                if (toDeploy > 0)
                {
                    GameObject go;

                    if (parent)
                        go = Instantiate(enemyWaves[currentWave].enemies[currentEnemy].gameObject, waveConfig.StartPointScript.GetPoint(), Quaternion.identity, parent);
                    else
                        go = Instantiate(enemyWaves[currentWave].enemies[currentEnemy].gameObject, waveConfig.StartPointScript.GetPoint(), Quaternion.identity);

                    deployedCount++;

                    if (setEndPoints)
                    {
                        SetEndPoint(go);
                    }
                }

                // This means we deployed enough troops for the current enemy so we have to reset some flags for the next enemy.
                if (deployedCount == toDeploy)
                {
                    calculatedTroops = false;
                    toDeploy = 0;
                    deployedCount = 0;
                    currentEnemy++;
                }

                yield return new WaitForSeconds(spawnRate);
            }

            // If this is the last deployment we want to reset the deployments and increase the wave counter.
            if (currentDeployment == enemyWaves[currentWave].deployments)
            {
                this.currentWave++;
                currentDeployment = 1;

                if (currentWave != enemyWaves.Count - 1 && eventWaveInProgress != null)
                    eventWaveInProgress.Raise(false);
            }
            else
            {
                currentDeployment++;

                float randomTime = Random.Range(minTimeForNextDeployment, maxTimeForNextDeployment);
                Invoke("StartWave", randomTime);
            }
        }

        public void SetEndPoint(GameObject gameObject)
        {
            gameObject.AddComponent(typeof(SetAgentDestination));
            SetAgentDestination sad = gameObject.GetComponent<SetAgentDestination>();
            List<Transform> result = endPoints.GetEndPoints(gameObject);

            if (result != null)
                sad.CalculateValidPath(result);
        }

        #region Custom object structs
        [System.Serializable]
        public struct SingleWave
        {
            [HideInInspector] public string name;
            public List<EnemyCount> enemies;
            public int deployments;
        }

        [System.Serializable]
        public struct EnemyCount
        {
            [HideInInspector] public string name;
            public GameObject gameObject;
            public int amount;
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
                        amount = enemyWaves[i].enemies[j].amount
                    };

                    if (enemyWaves[i].enemies[j].amount < 1)
                        enemyWaves[i].enemies[j] = new EnemyCount
                        {
                            name = enemyWaves[i].enemies[j].name,
                            gameObject = enemyWaves[i].enemies[j].gameObject,
                            amount = 1
                        };
                }

                enemyWaves[i] = new SingleWave
                {
                    name = "Wave " + (i + 1),
                    enemies = enemyWaves[i].enemies,
                    deployments = enemyWaves[i].deployments
                };

                if (enemyWaves[i].deployments < 1)
                    enemyWaves[i] = new SingleWave
                    {
                        name = enemyWaves[i].name,
                        enemies = enemyWaves[i].enemies,
                        deployments = 1
                    };
            }
        }
        #endregion
    }
}