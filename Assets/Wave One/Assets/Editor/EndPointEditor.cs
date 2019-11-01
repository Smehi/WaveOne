using SemihOrhan.WaveOne.EndPoints;
using UnityEditor;
using UnityEngine;

namespace SemihOrhan.WaveOne.CustomEditors
{
    [CustomEditor(typeof(EndPoint))]
    public class EndPointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject so = new SerializedObject(target);
            so.Update();

            SerializedProperty showEndPointsListControls = so.FindProperty("showEndPointsListControls");
            SerializedProperty showEnemiesListControls = so.FindProperty("showEnemiesListControls");
            SerializedProperty showNoEndPointEnemiesListControls = so.FindProperty("showNoEndPointEnemiesListControls");
            SerializedProperty endPoints = so.FindProperty("endPoints");
            SerializedProperty addColliders = so.FindProperty("addColliders");
            SerializedProperty triggerColliders = so.FindProperty("triggerColliders");
            SerializedProperty colliderSize = so.FindProperty("colliderSize");
            SerializedProperty noEndPointEnemies = so.FindProperty("noEndPointEnemies");

            EditorGUILayout.PropertyField(showEndPointsListControls);
            EditorGUILayout.PropertyField(showEnemiesListControls);
            EditorGUILayout.PropertyField(showNoEndPointEnemiesListControls);

            // List<EndPoint>
            EditorGUILayout.PropertyField(endPoints);
            EditorGUI.indentLevel++;
            if (endPoints.isExpanded)
            {
                EditorButtons.ShowAddButton(endPoints);

                for (int i = 0; i < endPoints.arraySize; i++)
                {
                    // List<EndPoint> list element
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(endPoints.GetArrayElementAtIndex(i));
                    if (showEndPointsListControls.boolValue && EditorButtons.ShowElementButtons(endPoints, i))
                        continue;
                    EditorGUILayout.EndHorizontal();

                    SerializedProperty endPoint = endPoints.GetArrayElementAtIndex(i).FindPropertyRelative("endPoint");
                    SerializedProperty enemies = endPoints.GetArrayElementAtIndex(i).FindPropertyRelative("enemies");

                    EditorGUI.indentLevel++;
                    if (endPoints.GetArrayElementAtIndex(i).isExpanded)
                    {
                        EditorGUILayout.PropertyField(endPoint);
                        // EndPoint List<GameObject>
                        EditorGUILayout.PropertyField(enemies);

                        EditorGUI.indentLevel++;
                        if (enemies.isExpanded)
                        {
                            EditorButtons.ShowAddButton(enemies);

                            for (int j = 0; j < enemies.arraySize; j++)
                            {
                                // EndPoint List<GameObject> list element
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.PropertyField(enemies.GetArrayElementAtIndex(j), new GUIContent("Enemy " + (j + 1)));
                                if (showEnemiesListControls.boolValue && EditorButtons.ShowElementButtons(enemies, j))
                                    continue;
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(addColliders, true);

            using (var group = new EditorGUILayout.FadeGroupScope(System.Convert.ToSingle(addColliders.boolValue)))
            {
                if (group.visible)
                {
                    EditorGUILayout.PropertyField(triggerColliders, true);
                    EditorGUILayout.PropertyField(colliderSize, true);
                }
            }

            EditorGUILayout.PropertyField(noEndPointEnemies);
            EditorGUI.indentLevel++;
            if (noEndPointEnemies.isExpanded)
            {
                EditorButtons.ShowAddButton(noEndPointEnemies);

                for (int i = 0; i < noEndPointEnemies.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(noEndPointEnemies.GetArrayElementAtIndex(i), new GUIContent("Enemy " + (i + 1)));
                    if (showNoEndPointEnemiesListControls.boolValue && EditorButtons.ShowElementButtons(noEndPointEnemies, i))
                        continue;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUI.indentLevel--;

            so.ApplyModifiedProperties();
        }
    }
}
