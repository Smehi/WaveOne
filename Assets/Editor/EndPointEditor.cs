using SemihOrhan.WaveOne.EndPoints;
using UnityEditor;

namespace SemihOrhan.WaveOne.CustomEditors
{
    [CustomEditor(typeof(EndPoint))]
    public class EndPointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject so = new SerializedObject(target);

            SerializedProperty endPoints = so.FindProperty("endPoints");
            SerializedProperty addColliders = so.FindProperty("addColliders");
            SerializedProperty triggerColliders = so.FindProperty("triggerColliders");
            SerializedProperty colliderSize = so.FindProperty("colliderSize");
            SerializedProperty noEndPointEnemies = so.FindProperty("noEndPointEnemies");

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target as EndPoint), typeof(EndPoint), false);
            }

            EditorGUILayout.PropertyField(endPoints, true);
            EditorGUILayout.PropertyField(addColliders, true);

            using (var group = new EditorGUILayout.FadeGroupScope(System.Convert.ToSingle(addColliders.boolValue)))
            {
                if (group.visible)
                {
                    EditorGUILayout.PropertyField(triggerColliders, true);
                    EditorGUILayout.PropertyField(colliderSize, true);
                }
            }

            EditorGUILayout.PropertyField(noEndPointEnemies, true);

            so.ApplyModifiedProperties();
        }
    } 
}
