using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDatabase : ScriptableObject
{
    [field: SerializeField] public GameObject[] units;
}



