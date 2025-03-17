using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", menuName = "Scriptable Objects/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    [Tooltip("The speed at which the projectile moves.")]
    [Range(5, 50)]
    public int speed;
    [Tooltip("The rate at which the projectile slows down.")]
    [Range(0.005f, 0.5f)]
    public float dampener;
    [Tooltip("How much damage the projectile does.")]
    [Range(1, 10)]
    public int attackDamage;
    [Tooltip("How much damage the projectile does. (Only X and Z variables should be changed)")]
    [Range(0f, 5f)]
    public float spread;
    [Tooltip("How many seconds does the projectile live for.")]
    [Range(0f, 5f)]
    public float lifeTimer;
}
