using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/Enemy/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Movement")]
    [Tooltip("The speed at which the enemy moves.")]
    [Range(1, 20)]
    public int movementSpeed;
    [Tooltip("The rate at which the enemy slows down.")]
    [Range(0.005f, 0.5f)]
    public float dampener;
    [Tooltip("The rate at which the enemy changes direction.")]
    [Range(0.005f, 0.5f)]
    public float smoothener;

    [Header("Attacking")]
    [Tooltip("The range the enemy can see the player.")]
    [Range(1f, 20f)]
    public float sightRange;
    [Tooltip("The range the enemy can attack the player at.")]
    [Range(1f, 20f)]
    public float attackRange;
    [Tooltip("The Enemy weapon/projectile.")]
    public EnemyProjectile enemyProjectile;
    [Tooltip("The Enemy's weapon type.")]
    public GunType gunType;
    [Tooltip("The amount of shots per fire")]
    [Range(1, 10)]
    public int shotsPerFire;

    [Header("Health")]
    [Tooltip("How many hits until the enemy dies.")]
    [Range(1, 50)]
    public int hitPoints;

    [Header("Timers")]
    [Tooltip("How long until the enemy despawns without seeing the player.")]
    [Range(1, 5)]
    public int despawnTimer;
    [Tooltip("The speed at which the enemy attacks.")]
    [Range(0.005f, 50f)]
    public float attacksPerSecond;
    [Tooltip("The rate at which the enemy looks for the player.")]
    [Range(1f, 20f)]
    public float searchRate;
}
