using SemihOrhan.WaveOne.Spawners;
using UnityEditor;

namespace SemihOrhan.WaveOne.CustomEditors
{
    [CustomEditor(typeof(TimedSpawner))]
    public class TimedSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject so = new SerializedObject(target);
            so.Update();

            SerializedProperty showEnemiesListControls = so.FindProperty("showEnemiesListControls");
            SerializedProperty enemyList = so.FindProperty("enemyList");
            SerializedProperty maxTime = so.FindProperty("maxTime");
            SerializedProperty spawnRate = so.FindProperty("spawnRate");
            SerializedProperty enemyParentObject = so.FindProperty("enemyParentObject");
            SerializedProperty eventSpawnerFinished = so.FindProperty("eventSpawnerFinished");
            SerializedProperty eventDeployedEnemies = so.FindProperty("eventDeployedEnemies");
            SerializedProperty eventAliveEnemies = so.FindProperty("eventAliveEnemies");

            EditorGUILayout.PropertyField(showEnemiesListControls, true);

            // List<EnemyWithWeight>
            EditorGUILayout.PropertyField(enemyList);
            EditorGUI.indentLevel++;
            if (enemyList.isExpanded)
            {
                EditorButtons.ShowAddButton(enemyList);

                for (int i = 0; i < enemyList.arraySize; i++)
                {
                    // List<EnemyWithWeight> list element
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(enemyList.GetArrayElementAtIndex(i));
                    if (showEnemiesListControls.boolValue && EditorButtons.ShowElementButtons(enemyList, i))
                        continue;
                    EditorGUILayout.EndHorizontal();

                    SerializedProperty enemies = enemyList.GetArrayElementAtIndex(i).FindPropertyRelative("gameObject");
                    SerializedProperty groupSize = enemyList.GetArrayElementAtIndex(i).FindPropertyRelative("groupSize");
                    SerializedProperty weight = enemyList.GetArrayElementAtIndex(i).FindPropertyRelative("weight");

                    // EnemyWithWeight elements
                    EditorGUI.indentLevel++;
                    if (enemyList.GetArrayElementAtIndex(i).isExpanded)
                    {
                        EditorGUILayout.PropertyField(enemies);
                        EditorGUILayout.PropertyField(groupSize);
                        EditorGUILayout.PropertyField(weight);
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(maxTime);
            EditorGUILayout.PropertyField(spawnRate);
            EditorGUILayout.PropertyField(enemyParentObject);
            EditorGUILayout.PropertyField(eventSpawnerFinished);
            EditorGUILayout.PropertyField(eventDeployedEnemies);
            EditorGUILayout.PropertyField(eventAliveEnemies);

            so.ApplyModifiedProperties();
        }
    }

}