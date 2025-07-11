using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class PartyDatabaseScript : ScriptableObject
{
    [field: SerializeField]
    public GameObject[] playerParty;
}
