using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Scriptable Objects/Enemy/TowerStats")]
public class TowerStats : ScriptableObject
{
    [Tooltip("The rate at which the tower uses it's ability.")]
    public float useRate;
    [Tooltip("How many hits until the tower dies.")]
    public int hitPoints;
    [Tooltip("How far can the tower see.")]
    public float visualRange;
}
