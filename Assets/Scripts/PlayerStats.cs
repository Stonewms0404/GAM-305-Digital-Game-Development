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
    [Tooltip("The rate at which the player changes direction.")]
    [Range(0.005f, 0.5f)]
    public float smoothener;

    [Header("Shooting")]
    [Tooltip("How many shots can the player shoot out per second.")]
    [Range(1, 100)]
    public int shootsPerSecond;

    [Header("Health")]
    [Tooltip("How many hits until the player dies.")]
    public float lives;
}
