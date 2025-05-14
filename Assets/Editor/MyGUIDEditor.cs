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

    private bool editGUID = false;
    public override void OnInspectorGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            if (editGUID)
            {
                EditorGUILayout.PropertyField(_GUID);
                //GUILayout.FlexibleSpace();
            }
            else
            {
                GUILayout.Label($"GUID:", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.Label($"( {_GUID.stringValue} )");
            }
        }
        /*
        if (GUILayout.Button("Edit GUID"))
        {
            editGUID = (editGUID == true ? false : true);
        }
        */
    }
}
