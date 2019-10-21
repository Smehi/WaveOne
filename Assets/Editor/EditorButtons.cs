using UnityEngine;
using UnityEditor;
using System;

namespace SemihOrhan.WaveOne.CustomEditors
{
    public static class EditorButtons
    {
        private static GUIContent moveUpButtonContent = new GUIContent("\u2191", "move element up");
        private static GUIContent moveDownButtonContent = new GUIContent("\u2193", "move element down");
        private static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate element");
        private static GUIContent removeButtonContent = new GUIContent("-", "remove element");
        private static GUIContent addButtonContent = new GUIContent("+", "add element");
        private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

        public static void ShowAddButton(SerializedProperty list)
        {
            if (list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
                list.arraySize++;
        }

        public static bool ShowElementButtons(SerializedProperty list, int index)
        {
            if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
                list.InsertArrayElementAtIndex(index);
            if (GUILayout.Button(removeButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
            { list.DeleteArrayElementAtIndex(index); return true; }
            if (GUILayout.Button(moveUpButtonContent, EditorStyles.miniButtonMid, miniButtonWidth) && index != 0)
                list.MoveArrayElement(index, --index);
            if (GUILayout.Button(moveDownButtonContent, EditorStyles.miniButtonRight, miniButtonWidth) && index != list.arraySize - 1)
                list.MoveArrayElement(index, ++index);

            return false;
        }
    }
}