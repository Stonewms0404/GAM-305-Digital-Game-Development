using UnityEngine;

public class TurretTower : Tower
{
    #region Variables
    [SerializeField] int shotsPerFire;
    [SerializeField] EnemyProjectile[] projectiles;

    float shootTimer;
    bool canShoot;
    #endregion

    #region Unity Functions

    #endregion

    #region Other Functions
    protected override void UseAbility()
    {
        canShoot = shootTimer <= 0;
        if (canShoot && playerInRange)
        {
            Shoot();
            shootTimer = 1f / stats.useRate;
            canShoot = false;
        }
        else
            shootTimer -= Time.deltaTime;
    }
    void Shoot()
    {
        int index = Random.Range(0, projectiles.Length);
        for (int i = 0; i < Random.Range(0, shotsPerFire) + 1; i++)
            Instantiate(projectiles[index], transform.position,
                projectiles[index].transform.rotation, projectileContainer.transform);
    }
    #endregion
}
