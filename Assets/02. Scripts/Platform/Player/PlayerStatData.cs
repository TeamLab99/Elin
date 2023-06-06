using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerStatDatas",menuName = "ScriptableObjects / PlayerStatDatas",order =1)]
public class PlayerStatData : ScriptableObject
{
    public int currentHP;
    public int maxHP;
    public int attackPower;
    public int maxCost;
    public float costRecoverySpeed;
    public float currentMP;
    public float maxMP;
}
