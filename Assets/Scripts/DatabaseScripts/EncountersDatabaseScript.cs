using System;
using UnityEngine;

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
