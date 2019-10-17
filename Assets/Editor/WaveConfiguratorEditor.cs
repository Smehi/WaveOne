using UnityEditor;
using UnityEngine;
using SemihOrhan.WaveOne.Spawners;

namespace SemihOrhan.WaveOne.CustomEditors
{
    [CustomEditor(typeof(WaveConfigurator))]
    public class WaveConfiguratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            WaveConfigurator waveConfig = target as WaveConfigurator;
            SerializedObject so = new SerializedObject(target);
            so.Update();

            SerializedProperty startPointType = so.FindProperty("startPointType");
            SerializedProperty startPointPickerType = so.FindProperty("startPointPickerType");
            SerializedProperty spawnerType = so.FindProperty("spawnerType");
            SerializedProperty spawnerPickerType = so.FindProperty("spawnerPickerType");
            SerializedProperty endPointsType = so.FindProperty("endPointsType");

            EditorGUILayout.PropertyField(startPointType, true);
            EditorGUILayout.PropertyField(startPointPickerType, true);

            EditorGUILayout.PropertyField(spawnerType, true);
            if (spawnerType.enumValueIndex == (int)SpawnerEnum.SpawnerType.PerWaveCustom)
            {
                EditorGUILayout.PropertyField(spawnerPickerType, true); 
            }

            EditorGUILayout.PropertyField(endPointsType, true);

            Color oldColor = GUI.backgroundColor;
            if (GUILayout.Button("Add components (and destroy old)"))
            {
                waveConfig.AddStartPointComponents();
                waveConfig.AddSpawnerComponents();
                waveConfig.AddEndPointComponents();
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Remove all components"))
            {
                if (EditorUtility.DisplayDialog("Delete components",
                                                "You are about to delete all components other than this (WaveConfigurator), are you sure?",
                                                "Delete",
                                                "Cancel"))
                {
                    waveConfig.RemoveAllComponents();
                }
            }
            GUI.backgroundColor = oldColor;

            so.ApplyModifiedProperties();
        }
    } 
}
