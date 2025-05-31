using UnityEngine;

[CreateAssetMenu]
public class EnemyDatabase : ScriptableObject
{
    [field: SerializeField] public Enemies[] units;
}

public class Enemies
{
    public string unitName;
    public GameObject UnitObj;
}



