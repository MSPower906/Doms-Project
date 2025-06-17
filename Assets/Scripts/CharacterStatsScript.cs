using System;
using UnityEngine;

public class CharacterStatsScript:MonoBehaviour
{ 
    [Header("Name/Type")]
    public string unitName;
    public string unitType;
    public bool playerCharacter;

    [Space(5)]
    [Header("Stats")]
    public int unitStr;
    public int unitVit;
    public int unitSpd;
    public int unitWis;
    public int unitCha;


    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;


    [Space(5)]
    [Header("Abilities")]
    public Abilities[] abilities;

    [Space(10)]
    public Resistances resistances;

}

[Serializable]
public class Abilities
{
    public string abilName;
    public string abilDesc;
    public bool targetEnemy;
    public Classification abilType;
    public int damage;
    public int manaCost;

    public ActionEffects abilEffect;

}