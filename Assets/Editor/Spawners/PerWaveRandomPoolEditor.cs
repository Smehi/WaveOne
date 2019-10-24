using SemihOrhan.WaveOne.Spawners;
using UnityEditor;

namespace SemihOrhan.WaveOne.CustomEditors
{
    [CustomEditor(typeof(PerWaveRandomPool))]
    public class PerWaveRandomPoolEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject so = new SerializedObject(target);
            so.Update();

            SerializedProperty showWaveListControls = so.FindProperty("showWaveListControls");
            SerializedProperty showEnemiesListControls = so.FindProperty("showEnemiesListControls");
            SerializedProperty enemyWaves = so.FindProperty("enemyWaves");
            SerializedProperty minTimeForNextDeployment = so.FindProperty("minTimeForNextDeployment");
            SerializedProperty maxTimeForNextDeployment = so.FindProperty("maxTimeForNextDeployment");
            SerializedProperty spawnRate = so.FindProperty("spawnRate");
            SerializedProperty autoDeploy = so.FindProperty("autoDeploy");
            SerializedProperty enemyParentObject = so.FindProperty("enemyParentObject");
            SerializedProperty eventSpawnerFinished = so.FindProperty("eventSpawnerFinished");
            SerializedProperty eventTotalEnemies = so.FindProperty("eventTotalEnemies");
            SerializedProperty eventDeployedEnemies = so.FindProperty("eventDeployedEnemies");
            SerializedProperty eventAliveEnemies = so.FindProperty("eventAliveEnemies");

            EditorGUILayout.PropertyField(showWaveListControls, true);
            EditorGUILayout.PropertyField(showEnemiesListControls, true);

            // List<SingleWave>
            EditorGUILayout.PropertyField(enemyWaves);
            EditorGUI.indentLevel++;
            if (enemyWaves.isExpanded)
            {
                EditorButtons.ShowAddButton(enemyWaves);

                for (int i = 0; i < enemyWaves.arraySize; i++)
                {
                    // List<SingleWave> list element
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(enemyWaves.GetArrayElementAtIndex(i));
                    if (showWaveListControls.boolValue && EditorButtons.ShowElementButtons(enemyWaves, i))
                        continue;
                    EditorGUILayout.EndHorizontal();

                    SerializedProperty enemies = enemyWaves.GetArrayElementAtIndex(i).FindPropertyRelative("enemies");
                    SerializedProperty poolSize = enemyWaves.GetArrayElementAtIndex(i).FindPropertyRelative("poolSize");
                    SerializedProperty deployments = enemyWaves.GetArrayElementAtIndex(i).FindPropertyRelative("deployments");

                    // SingleWave List<EnemyCount>
                    EditorGUI.indentLevel++;
                    if (enemyWaves.GetArrayElementAtIndex(i).isExpanded)
                    {
                        EditorGUILayout.PropertyField(enemies);

                        EditorGUI.indentLevel++;
                        if (enemies.isExpanded)
                        {
                            EditorButtons.ShowAddButton(enemies);

                            for (int j = 0; j < enemies.arraySize; j++)
                            {
                                // SingleWave List<EnemyCount> list element
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.PropertyField(enemies.GetArrayElementAtIndex(j));
                                if (showEnemiesListControls.boolValue && EditorButtons.ShowElementButtons(enemies, j))
                                    continue;
                                EditorGUILayout.EndHorizontal();

                                SerializedProperty gameObject = enemies.GetArrayElementAtIndex(j).FindPropertyRelative("gameObject");
                                SerializedProperty groupSize = enemies.GetArrayElementAtIndex(j).FindPropertyRelative("groupSize");

                                // EnemyCount values
                                EditorGUI.indentLevel++;
                                if (enemies.GetArrayElementAtIndex(j).isExpanded)
                                {
                                    EditorGUILayout.PropertyField(gameObject);
                                    EditorGUILayout.PropertyField(groupSize);
                                }
                                EditorGUI.indentLevel--;
                            }
                        }
                        EditorGUI.indentLevel--;

                        // SingleWave poolSize and deployments
                        EditorGUILayout.PropertyField(poolSize);
                        EditorGUILayout.PropertyField(deployments);
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(minTimeForNextDeployment);
            EditorGUILayout.PropertyField(maxTimeForNextDeployment);
            EditorGUILayout.PropertyField(spawnRate);
            EditorGUILayout.PropertyField(autoDeploy);
            EditorGUILayout.PropertyField(enemyParentObject);
            EditorGUILayout.PropertyField(eventSpawnerFinished);
            EditorGUILayout.PropertyField(eventTotalEnemies);
            EditorGUILayout.PropertyField(eventDeployedEnemies);
            EditorGUILayout.PropertyField(eventAliveEnemies);

            so.ApplyModifiedProperties();
        }
    }

}