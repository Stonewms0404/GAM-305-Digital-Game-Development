using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Tooltip("The speed at which the enemy moves.")]
    public float speed;
    [Tooltip("The rate at which the enemy slows down.")]
    public float dampener;
    [Tooltip("How many hits until the enemy dies.")]
    public float hitPoints;
}
