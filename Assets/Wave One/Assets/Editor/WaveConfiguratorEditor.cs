using SemihOrhan.WaveOne.Spawners;
using UnityEditor;
using UnityEngine;

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
            SerializedProperty needSpawnerPicker = so.FindProperty("needSpawnerPicker");
            SerializedProperty endPointsType = so.FindProperty("endPointsType");
            SerializedProperty formationType = so.FindProperty("formationType");

            EditorGUILayout.PropertyField(startPointType, true);
            EditorGUILayout.PropertyField(startPointPickerType, true);

            EditorGUILayout.PropertyField(spawnerType, true);
            needSpawnerPicker.boolValue = false;
            // We only want to show the spawnerPickerType when we have one of these spawners selected.
            if (spawnerType.enumValueIndex == (int)SpawnerEnum.SpawnerType.PerWaveCustom)
            {
                needSpawnerPicker.boolValue = true;
                EditorGUILayout.PropertyField(spawnerPickerType, true);
            }

            EditorGUILayout.PropertyField(endPointsType, true);
            EditorGUILayout.PropertyField(formationType, true);

            // If we do this after the buttons shit hits the fan.
            so.ApplyModifiedProperties();

            Color oldColor = GUI.backgroundColor;
            if (GUILayout.Button("Add components (and destroy old)"))
            {
                waveConfig.AddStartPointComponents();
                waveConfig.AddSpawnerComponents();
                waveConfig.AddEndPointComponents();
                waveConfig.AddFormationComponents();
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
        }
    } 
}
