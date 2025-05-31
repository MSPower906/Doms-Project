using System;
using UnityEngine;

public class CharacterStatsScript : MonoBehaviour
{
    [Header("Name/Type")]
    public string unitName;
    public string unitType;

    [Space(5)]
    [Header("Stats")]
    public float unitStr;
    public float unitVit;
    public float unitSpd;
    public float unitWis;
    public float unitCha;

    [Space(10)]
    [SerializeField] public Resistances resistances;

    private void Start()
    {
        
    }
}