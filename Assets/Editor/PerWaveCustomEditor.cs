using SemihOrhan.WaveOne.Spawners;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerWaveCustom))]
public class PerWaveCustomEditor : Editor
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
        SerializedProperty enemyParentObject = so.FindProperty("enemyParentObject");
        SerializedProperty eventWaveInProgress = so.FindProperty("eventWaveInProgress");
        SerializedProperty eventTotalEnemies = so.FindProperty("eventTotalEnemies");
        SerializedProperty eventDeployedEnemies = so.FindProperty("eventDeployedEnemies");
        SerializedProperty eventAliveEnemies = so.FindProperty("eventAliveEnemies");

        GUIContent moveUpButtonContent = new GUIContent("\u2191", "move element up");
        GUIContent moveDownButtonContent = new GUIContent("\u2193", "move element down");
        GUIContent duplicateButtonContent = new GUIContent("+", "duplicate element");
        GUIContent removeButtonContent = new GUIContent("-", "remove element");
        GUIContent addButtonContent = new GUIContent("+", "add element");
        GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

        EditorGUILayout.PropertyField(showWaveListControls);
        EditorGUILayout.PropertyField(showEnemiesListControls);

        // List<SingleWave>
        EditorGUILayout.PropertyField(enemyWaves);
        EditorGUI.indentLevel++;
        if (enemyWaves.isExpanded)
        {
            if (enemyWaves.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
                enemyWaves.arraySize++;

            for (int i = 0; i < enemyWaves.arraySize; i++)
            {
                // List<SingleWave> list element
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(enemyWaves.GetArrayElementAtIndex(i));
                if (showWaveListControls.boolValue)
                {
                    if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
                        enemyWaves.InsertArrayElementAtIndex(i);
                    if (GUILayout.Button(removeButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
                    { enemyWaves.DeleteArrayElementAtIndex(i); continue; }
                    if (GUILayout.Button(moveUpButtonContent, EditorStyles.miniButtonMid, miniButtonWidth) && i != 0)
                        enemyWaves.MoveArrayElement(i, --i);
                    if (GUILayout.Button(moveDownButtonContent, EditorStyles.miniButtonRight, miniButtonWidth) && i != enemyWaves.arraySize - 1)
                        enemyWaves.MoveArrayElement(i, ++i);
                }
                EditorGUILayout.EndHorizontal();

                SerializedProperty enemies = enemyWaves.GetArrayElementAtIndex(i).FindPropertyRelative("enemies");
                SerializedProperty deployments = enemyWaves.GetArrayElementAtIndex(i).FindPropertyRelative("deployments");

                // SingleWave List<EnemyCount>
                EditorGUI.indentLevel++;
                if (enemyWaves.GetArrayElementAtIndex(i).isExpanded)
                {
                    EditorGUILayout.PropertyField(enemies);

                    EditorGUI.indentLevel++;
                    if (enemies.isExpanded)
                    {
                        if (enemies.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
                            enemies.arraySize++;

                        for (int j = 0; j < enemies.arraySize; j++)
                        {
                            // SingleWave List<EnemyCount> list element
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PropertyField(enemies.GetArrayElementAtIndex(j));
                            if (showEnemiesListControls.boolValue)
                            {
                                if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
                                    enemies.InsertArrayElementAtIndex(j);
                                if (GUILayout.Button(removeButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
                                { enemies.DeleteArrayElementAtIndex(j); continue; }
                                if (GUILayout.Button(moveUpButtonContent, EditorStyles.miniButtonMid, miniButtonWidth) && j != 0)
                                    enemies.MoveArrayElement(j, --j);
                                if (GUILayout.Button(moveDownButtonContent, EditorStyles.miniButtonRight, miniButtonWidth) && j != enemies.arraySize - 1)
                                    enemies.MoveArrayElement(j, ++j);
                            }
                            EditorGUILayout.EndHorizontal();

                            SerializedProperty gameObject = enemies.GetArrayElementAtIndex(j).FindPropertyRelative("gameObject");
                            SerializedProperty amount = enemies.GetArrayElementAtIndex(j).FindPropertyRelative("amount");
                            SerializedProperty groupSize = enemies.GetArrayElementAtIndex(j).FindPropertyRelative("groupSize");

                            // EnemyCount values
                            EditorGUI.indentLevel++;
                            if (enemies.GetArrayElementAtIndex(j).isExpanded)
                            {
                                EditorGUILayout.PropertyField(gameObject);
                                EditorGUILayout.PropertyField(amount);
                                EditorGUILayout.PropertyField(groupSize);
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                    EditorGUI.indentLevel--;

                    // SingleWave deployments
                    EditorGUILayout.PropertyField(deployments);
                }
                EditorGUI.indentLevel--;
            }
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(minTimeForNextDeployment, true);
        EditorGUILayout.PropertyField(maxTimeForNextDeployment, true);
        EditorGUILayout.PropertyField(spawnRate, true);
        EditorGUILayout.PropertyField(enemyParentObject, true);
        EditorGUILayout.PropertyField(eventWaveInProgress, true);
        EditorGUILayout.PropertyField(eventTotalEnemies, true);
        EditorGUILayout.PropertyField(eventDeployedEnemies, true);
        EditorGUILayout.PropertyField(eventAliveEnemies, true);

        so.ApplyModifiedProperties();
    }
}
