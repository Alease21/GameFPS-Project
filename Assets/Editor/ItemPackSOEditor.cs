using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using static WeaponSO;

[CustomEditor(typeof(ItemPackSO))]
public class ItemPackSOEditor : Editor
{
    public SerializedProperty _itemPackType;
    public SerializedProperty _itemPackName;
    public SerializedProperty _packAmount;
    public SerializedProperty _rechargeTime;
    public SerializedProperty _tickTime;
    public SerializedProperty _numTicks;

    private void OnEnable()
    {
        _itemPackType = serializedObject.FindProperty("itemPackType");
        _itemPackName = serializedObject.FindProperty("itemPackName");
        _packAmount = serializedObject.FindProperty("packAmount");
        _rechargeTime = serializedObject.FindProperty("rechargeTime");
        _tickTime = serializedObject.FindProperty("tickTime");
        _numTicks = serializedObject.FindProperty("numTicks");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(_itemPackName);
        EditorGUILayout.PropertyField(_itemPackType, new GUIContent("Item Pack Type"));
        EditorGUILayout.Space(10);

        switch (_itemPackType.enumValueIndex)
        {
            case 0: //health pack
                EditorGUILayout.LabelField("Health Pack Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_packAmount, new GUIContent("Heal Amount"));
                EditorGUILayout.PropertyField(_rechargeTime, new GUIContent("Pack Recharge Time"));
                break;
            case 1: //HOT pack
                EditorGUILayout.LabelField("HOT Pack Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_packAmount, new GUIContent($"Heal Amount (Per Tick)"));
                EditorGUILayout.PropertyField(_numTicks, new GUIContent("Number of Heal Ticks"));
                EditorGUILayout.PropertyField(_tickTime, new GUIContent("Tick Timer"));
                EditorGUILayout.PropertyField(_rechargeTime, new GUIContent("Pack Recharge Time"));
                break;
            case 2: //shield pack
                EditorGUILayout.LabelField("Shield Pack Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_packAmount, new GUIContent("Shield Amount"));
                EditorGUILayout.PropertyField(_rechargeTime, new GUIContent("Pack Recharge Time"));
                break;
            case 3: //ammo pack
                EditorGUILayout.LabelField("Ammo Pack Info:", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_packAmount, new GUIContent("Ammo Amount (All Weapons)"));
                break;
        }
    }
}
