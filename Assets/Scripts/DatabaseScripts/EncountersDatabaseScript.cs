using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class EncountersDatabaseScript : ScriptableObject
{

    [field: SerializeField] public Encounters[] encounters;

}

[Serializable]
public class Encounters
{
    public string encounterName;
    public GameObject[] enemies;
}

