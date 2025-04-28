using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "NewEnemy/NewEvironEnemy")]
public class EnvironmentalEnemySO : ScriptableObject
{
    public enum EnvironEnemyType
    {
        Explosive,
        Lava,
        BreakableWall
    }
    public EnvironEnemyType environEnemyType;
    public string environEnemyName;
    public int health;
    public int damage;
}