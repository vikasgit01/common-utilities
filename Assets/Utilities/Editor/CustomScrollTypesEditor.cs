using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace GameUIUtilities
{

    [CustomEditor(typeof(CustomScrollTypes))]
    public class CustomScrollTypesEditor : ScrollRectEditor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            SerializedProperty scrollType = serializedObject.FindProperty("scrollType");
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("scrollType"), false);

            if (scrollType.enumValueIndex != (int)ScrollType.NONE)
            {
                if (scrollType.enumValueIndex == (int)ScrollType.TRAVERSEINDICATOR
                  || scrollType.enumValueIndex == (int)ScrollType.TRAVERSE || scrollType.enumValueIndex == (int)ScrollType.INDICATORCLICK)
                {

                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("axis"), false);
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("moveSpeed"), false);
                }

                if (scrollType.enumValueIndex == (int)ScrollType.TRAVERSEINDICATOR
                    || scrollType.enumValueIndex == (int)ScrollType.INDICATOR || scrollType.enumValueIndex == (int)ScrollType.INDICATORCLICK)
                {
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("indicatorContent"), false);
                }
            }

            GUILayout.Space(10);

            this.serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}