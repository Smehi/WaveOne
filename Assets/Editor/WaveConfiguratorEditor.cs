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

        if (GUILayout.Button("Add components (and destroy old)"))
        {
            waveConfig.AddComponents();
        }
    }
}
