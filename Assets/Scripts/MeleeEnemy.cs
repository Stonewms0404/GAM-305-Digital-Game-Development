using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    #region Variables
    GameObject weapon;
    #endregion

    #region Unity Functions

    #endregion

    #region Other Functions
    protected override void Attack()
    {
        canAttack = attackTimer <= 0;
        if (canAttack)
        {
            Swing();
            attackTimer = 1f / stats.attacksPerSecond;
        }
        else
            attackTimer -= Time.deltaTime;
    }
    async void Swing()
    {
        Destroy(weapon);
        var end = Time.time + stats.attacksPerSecond;
        weapon = Instantiate(stats.enemyProjectile, transform.position,
            stats.enemyProjectile.transform.rotation, transform).gameObject;
        Destroy(weapon, stats.attacksPerSecond);
        while (Time.time < end)
        {
            if (weapon)
            {
                weapon.transform.Rotate(stats.enemyProjectile.stats.speed * 30f * Time.deltaTime * Vector3.up);
                weapon.transform.position = transform.position;
            }
            await Task.Yield();
        }
    }
    #endregion
}
