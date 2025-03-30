using System.Net;
using UnityEngine;

public class RangedEnemy : Enemy
{
    #region Variables

    #endregion

    #region Unity Functions
    #endregion

    #region Other Functions
    protected override void Attack()
    {
        canAttack = attackTimer <= 0;
        if (canAttack)
        {
            Shoot();
            attackTimer = 1f / stats.attacksPerSecond;
        }
        else
            attackTimer -= Time.deltaTime;
    }
    void Shoot()
    {
        switch (stats.gunType)
        {
            case GunType.Homing:
                for (int i = 0; i < Random.Range(1, stats.shotsPerFire + 1); i++)
                    Instantiate(stats.enemyProjectile, transform.position,
                        stats.enemyProjectile.transform.rotation, projectileContainer.transform);
                break;
            case GunType.Shotgun:
                for (int i = 0; i < stats.shotsPerFire; i++)
                    Instantiate(stats.enemyProjectile, transform.position,
                        stats.enemyProjectile.transform.rotation, projectileContainer.transform);
                break;
            default:
                Instantiate(stats.enemyProjectile, transform.position,
                    stats.enemyProjectile.transform.rotation, projectileContainer.transform);
                break;
        }
    }
    #endregion
}
