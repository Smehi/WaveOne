using SemihOrhan.WaveOne.StartPoints;
using UnityEditor;
using UnityEngine;

namespace SemihOrhan.WaveOne.CustomEditors
{
    [CustomEditor(typeof(ListOfSpheres))]
    public class ListOfSpheresEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject so = new SerializedObject(target);
            so.Update();

            SerializedProperty startPoints = so.FindProperty("startPoints");
            SerializedProperty drawGizmos = so.FindProperty("drawGizmos");

            EditorGUILayout.PropertyField(startPoints);
            EditorGUI.indentLevel++;
            if (startPoints.isExpanded)
            {
                EditorButtons.ShowAddButton(startPoints);

                for (int i = 0; i < startPoints.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(startPoints.GetArrayElementAtIndex(i), new GUIContent("Start point " + (i + 1)));
                    if (EditorButtons.ShowElementButtons(startPoints, i))
                        continue;
                    EditorGUILayout.EndHorizontal();

                    SerializedProperty transform = startPoints.GetArrayElementAtIndex(i).FindPropertyRelative("transform");
                    SerializedProperty radius = startPoints.GetArrayElementAtIndex(i).FindPropertyRelative("radius");
                    SerializedProperty minDistanceFromCenter = startPoints.GetArrayElementAtIndex(i).FindPropertyRelative("minDistanceFromCenter");

                    EditorGUI.indentLevel++;
                    if (startPoints.GetArrayElementAtIndex(i).isExpanded)
                    {
                        EditorGUILayout.PropertyField(transform);
                        EditorGUILayout.PropertyField(radius);
                        EditorGUILayout.PropertyField(minDistanceFromCenter);
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(drawGizmos);

            so.ApplyModifiedProperties();
        }
    }
}