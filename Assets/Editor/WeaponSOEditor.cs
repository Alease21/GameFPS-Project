using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponSO))]
public class WeaponSOEditor : Editor
{
    public SerializedProperty _weaponName;
    public SerializedProperty _weaponType;
    public SerializedProperty _ammoMax;
    public SerializedProperty _ammoCount;
    public SerializedProperty _damage;
    public SerializedProperty _range;
    public SerializedProperty _attackAnimation;
    public SerializedProperty _hitBoxPrefab;
    public SerializedProperty _explodeTime;

    private void OnEnable()
    {
        _weaponName = serializedObject.FindProperty("weaponName");
        _weaponType = serializedObject.FindProperty("weaponType");
        _ammoMax = serializedObject.FindProperty("ammoMax");
        _ammoCount = serializedObject.FindProperty("ammoCount");
        _damage = serializedObject.FindProperty("damage");
        _range = serializedObject.FindProperty("range");
        _attackAnimation = serializedObject.FindProperty("attackAnimation");
        _hitBoxPrefab = serializedObject.FindProperty("hitBoxPrefab");
        _explodeTime = serializedObject.FindProperty("explodeTime");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(_weaponName);
        EditorGUILayout.PropertyField(_weaponType, new GUIContent("Weapon Type"));
        EditorGUILayout.Space(10);

        switch (_weaponType.enumValueIndex)
        {
            case 0:
                EditorGUILayout.LabelField("Gun Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                EditorGUILayout.PropertyField(_damage, new GUIContent("Gun Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Gun Range"));
                EditorGUILayout.PropertyField(_attackAnimation, new GUIContent("Shoot Animation"));
                break;
            case 1:
                EditorGUILayout.LabelField("Gun Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                EditorGUILayout.PropertyField(_damage, new GUIContent("Gun Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Explode Range"));
                EditorGUILayout.PropertyField(_attackAnimation, new GUIContent("Shoot Animation"));
                break;
            case  2:
                EditorGUILayout.LabelField("Gun Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                EditorGUILayout.PropertyField(_damage, new GUIContent("Gun Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Gun Range"));
                EditorGUILayout.PropertyField(_attackAnimation, new GUIContent("Shoot Animation"));
                break;
            case 3:
                EditorGUILayout.LabelField("Melee Weapon Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_damage, new GUIContent("Weapon Damage"));
                //EditorGUILayout.PropertyField(_range, new GUIContent("Weapon Range"));
                EditorGUILayout.PropertyField(_attackAnimation, new GUIContent("Swing Animation"));
                EditorGUILayout.PropertyField(_hitBoxPrefab, new GUIContent("Weapon HitBox Prefab"));
                break;
            case 4:
                EditorGUILayout.LabelField("Throwable Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                EditorGUILayout.PropertyField(_damage, new GUIContent("Grenade Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Grenade Range"));
                EditorGUILayout.PropertyField(_explodeTime, new GUIContent("Grenade Timer"));
                break;
        }

        serializedObject.ApplyModifiedProperties(); //applies everything
    }
}
