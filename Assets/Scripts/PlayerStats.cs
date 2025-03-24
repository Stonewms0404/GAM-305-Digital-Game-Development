using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    [Tooltip("The speed at which the player moves.")]
    [Range(1, 20)]
    public int speed;
    [Tooltip("The rate at which the player slows down.")]
    [Range(0.005f, 0.5f)]
    public float dampener;
    [Tooltip("The rate at which the camera slows down.")]
    [Range(0f, 1f)]
    public float cameraDampener;
    [Tooltip("The rate at which the player changes direction.")]
    [Range(0.005f, 0.5f)]
    public float smoothener;

    [Header("Shooting")]
    [Tooltip("What type of shooting is the player doing.")]
    public GunType gunType;
    [Tooltip("How many shots can the player shoot out per second.")]
    [Range(0.005f, 50f)]
    public int shotsPerSecond;
    [Tooltip("The player projectile shot out.")]
    public PlayerProjectile playerProjectile;
    [Tooltip("The amount of shots per fire")]
    [Range(1, 10)]
    public int shotsPerFire;

    [Header("Health")]
    [Tooltip("How many hits until the enemy dies.")]
    [Range(1, 50)]
    public int hitPoints;
    [Tooltip("How many hits until the player dies.")]
    public int lives;
}

public enum GunType
{
    Pistol,
    Shotgun,
    AssaultRifle,
    Homing,
    MachineGun,
    Melee
}
