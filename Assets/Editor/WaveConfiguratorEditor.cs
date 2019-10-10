using UnityEngine;
using UnityEditor;
using WaveOne;

[CustomEditor(typeof(WaveConfigurator))]
public class WaveConfiguratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WaveConfigurator waveConfig = target as WaveConfigurator;

        Color oldColor = GUI.backgroundColor;

        if (GUILayout.Button("Add components (and destroy old)"))
        {
            waveConfig.AddStartPointComponents();
            waveConfig.AddSpawnerComponents();
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
