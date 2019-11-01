using UnityEditor;
using UnityEngine;

namespace SemihOrhan.WaveOne.CustomEditors
{
    [CustomEditor(typeof(WaveManager))]
    public class WaveManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject so = new SerializedObject(target);
            so.Update();

            SerializedProperty autoFindConfigs = so.FindProperty("autoFindConfigs");
            SerializedProperty showListControls = so.FindProperty("showListControls");
            SerializedProperty waveConfigurators = so.FindProperty("waveConfigurators");

            EditorGUILayout.PropertyField(autoFindConfigs);

            if (!autoFindConfigs.boolValue)
            {
                EditorGUILayout.PropertyField(showListControls);

                EditorGUILayout.PropertyField(waveConfigurators);
                EditorGUI.indentLevel++;
                if (waveConfigurators.isExpanded)
                {
                    EditorButtons.ShowAddButton(waveConfigurators);

                    for (int i = 0; i < waveConfigurators.arraySize; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(waveConfigurators.GetArrayElementAtIndex(i), new GUIContent("Wave config " + (i + 1)));
                        if (showListControls.boolValue && EditorButtons.ShowElementButtons(waveConfigurators, i))
                            continue;
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUI.indentLevel--;
            }

            so.ApplyModifiedProperties();
        }
    }
}
