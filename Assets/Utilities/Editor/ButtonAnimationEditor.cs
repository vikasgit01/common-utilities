using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine;

namespace GameUIUtilities
{
    [CustomEditor(typeof(ButtonAnimation))]
    public class ButtonAnimationEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("enableAnimation"), false);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("buttonEventName"), false);
            bool propertyField = serializedObject.FindProperty("enableAnimation").boolValue;

            if (propertyField)
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("animateGraphics"), false);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("scaleSize"), false);
            }

            this.serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}
