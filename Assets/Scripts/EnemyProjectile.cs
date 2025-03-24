using UnityEngine;

public class EnemyProjectile : Projectile
{
    #region Variables

    #endregion

    #region Other Functions
    protected override (bool success, Vector3 point) GetPosition()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        target = playerObj;
        return (true, target.transform.position);
    }
    protected override void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void AimTowardsPosition(Vector3 position)
    {
        Vector3 bulletSpread = new(Random.Range(-stats.spread, stats.spread), 0, Random.Range(-stats.spread, stats.spread));
        Vector3 direction = (position - transform.position).normalized;
        direction.y = 0;
        rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        rb.linearVelocity = direction * stats.speed + bulletSpread;

        Destroy(gameObject, stats.lifeTimer);
    }
    #endregion
}
