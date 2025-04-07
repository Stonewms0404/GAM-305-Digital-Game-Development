using UnityEngine;

public class TurretTower : Tower
{
    #region Variables
    [SerializeField] int shotsPerFire;
    [SerializeField] EnemyProjectile[] projectiles;
    [SerializeField] GameObject shootLocation;

    float shootTimer;
    bool canShoot;
    Vector3 direction;
    #endregion

    #region Unity Functions
    #endregion

    #region Other Functions
    protected override void UseAbility()
    {
        if (playerInRange)
        {
            direction = (transform.position - player.transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

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
            Instantiate(projectiles[index], shootLocation.transform.position,
                projectiles[index].transform.rotation, projectileContainer.transform);
    }
    #endregion
}
