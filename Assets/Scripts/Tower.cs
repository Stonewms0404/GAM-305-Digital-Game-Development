using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    #region Variables
    public TowerStats stats;
    [SerializeField] protected TowerStats[] listedStats;
    [Tooltip("The object that stores the scene's projectiles.")]
    protected GameObject projectileContainer;
    [Tooltip("The object that stores the scene's particles.")]
    protected GameObject particleContainer;
    [Tooltip("The object that stores the scene's enemies.")]
    protected GameObject enemyContainer;

    protected int health;
    protected bool playerInRange;
    GameObject? player;
    #endregion

    #region Unity Functions
    private void Start()
    {
        health = stats.hitPoints;
        player = GameObject.FindGameObjectWithTag("Player");

        projectileContainer = GameObject.FindGameObjectWithTag("ProjectileContainer");
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer");
        enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");
    }
    private void Update()
    {
        if (player)
        {
            float distance = (player.transform.position - transform.position).magnitude;
            playerInRange = distance <= stats.visualRange;
        }
        UseAbility();
    }
    #endregion

    #region Other Functions
    protected abstract void UseAbility();
    public void GotHit(int value)
    {
        health += value;
        if (health > stats.hitPoints)
            health = stats.hitPoints;
        else if (health <= 0)
            Death();
    }
    public void Death()
    {
        Destroy(gameObject);
    }
    #endregion
}
