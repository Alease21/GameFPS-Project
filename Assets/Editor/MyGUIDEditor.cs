using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MyGUID))]
public class MyGUIDEditor : Editor
{
    public SerializedProperty _GUID;

    private void OnEnable()
    {
        _GUID = serializedObject.FindProperty("_GUID");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"GUID:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"( {_GUID.stringValue} )");
        EditorGUILayout.EndHorizontal();
    }
}
