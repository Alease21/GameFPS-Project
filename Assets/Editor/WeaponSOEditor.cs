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
    public SerializedProperty _weaponPrefab;
    public SerializedProperty _projectilePrefab;
    public SerializedProperty _projectileSpeed;

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
        _weaponPrefab = serializedObject.FindProperty("weaponPrefab");
        _projectilePrefab = serializedObject.FindProperty("projectilePrefab");
        _projectileSpeed = serializedObject.FindProperty("projectileSpeed");
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
            case 0: //hitscan
                EditorGUILayout.LabelField("Gun Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                EditorGUILayout.PropertyField(_damage, new GUIContent("Gun Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Gun Range"));
                EditorGUILayout.PropertyField(_attackAnimation, new GUIContent("Shoot Animation"));
                EditorGUILayout.PropertyField(_weaponPrefab, new GUIContent("HitScan Gun Prefab"));
                EditorGUILayout.PropertyField(_projectilePrefab, new GUIContent("HitScan 'Shot' Prefab"));
                break;
            case 1: //projectile
                EditorGUILayout.LabelField("Gun Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                EditorGUILayout.PropertyField(_damage, new GUIContent("Gun Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Explode Range"));
                EditorGUILayout.PropertyField(_attackAnimation, new GUIContent("Shoot Animation"));
                EditorGUILayout.PropertyField(_weaponPrefab, new GUIContent("Projectile Gun Prefab"));
                EditorGUILayout.PropertyField(_projectilePrefab, new GUIContent("Projectile Prefab"));
                EditorGUILayout.PropertyField(_projectileSpeed, new GUIContent("Projectile Speed"));
                break;
            case  2: //continuous
                EditorGUILayout.LabelField("Gun Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                EditorGUILayout.PropertyField(_damage, new GUIContent("Gun Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Gun Range")); //use me for projectile deleting?
                EditorGUILayout.PropertyField(_attackAnimation, new GUIContent("Shoot Animation"));
                EditorGUILayout.PropertyField(_weaponPrefab, new GUIContent("Continuous Gun Prefab"));
                EditorGUILayout.PropertyField(_projectilePrefab, new GUIContent("Fire Projectile Prefab"));
                EditorGUILayout.PropertyField(_projectileSpeed, new GUIContent("Fire Projectile Speed"));
                break;
            case 3: //melee weapon
                EditorGUILayout.LabelField("Melee Weapon Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_damage, new GUIContent("Weapon Damage"));
                //EditorGUILayout.PropertyField(_range, new GUIContent("Weapon Range"));
                EditorGUILayout.PropertyField(_attackAnimation, new GUIContent("Swing Animation"));
                EditorGUILayout.PropertyField(_weaponPrefab, new GUIContent("Melee Weapon Prefab"));
                EditorGUILayout.PropertyField(_hitBoxPrefab, new GUIContent("Weapon HitBox Prefab"));

                break;
            case 4: //grenade throwable
                EditorGUILayout.LabelField("Throwable Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                EditorGUILayout.PropertyField(_damage, new GUIContent("Grenade Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Grenade Range"));
                EditorGUILayout.PropertyField(_explodeTime, new GUIContent("Grenade Timer"));
                EditorGUILayout.PropertyField(_weaponPrefab, new GUIContent("Grenade Prefab"));
                EditorGUILayout.PropertyField(_projectileSpeed, new GUIContent("Throwable Speed"));

                break;
            case 5: //smoke bomb throwable
                EditorGUILayout.LabelField("Throwable Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_ammoMax, new GUIContent("Max Ammo"));
                EditorGUILayout.PropertyField(_ammoCount, new GUIContent("Initial Ammo Count"));
                //EditorGUILayout.PropertyField(_damage, new GUIContent("Grenade Damage"));
                EditorGUILayout.PropertyField(_range, new GUIContent("Smoke Bomb Range"));
                EditorGUILayout.PropertyField(_explodeTime, new GUIContent("Smoke Bomb Timer"));
                EditorGUILayout.PropertyField(_weaponPrefab, new GUIContent("Smoke Bomb Prefab"));
                EditorGUILayout.PropertyField(_projectileSpeed, new GUIContent("Throwable Speed"));
                break;
        }

        serializedObject.ApplyModifiedProperties(); //applies everything
    }
}
