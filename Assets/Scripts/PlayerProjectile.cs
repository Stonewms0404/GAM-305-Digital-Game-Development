using System.Drawing;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    #region Variables

    #endregion

    #region Other Functions
    protected override (bool success, Vector3 point) GetPosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo))
            return (true, hitInfo.point);
        else
            return (false, Vector3.zero);
    }

    protected override void GetTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            return;
        GameObject closest = enemies[0];
        float distance = Mathf.Infinity;
        for (int i = 0; i < enemies.Length; i++)
        {
            float magnitude = (enemies[i].transform.position - transform.position).magnitude;
            if (magnitude < distance)
            {
                distance = magnitude;
                closest = enemies[i];
            }
        }
        target = closest;
    }

    protected override void AimTowardsPosition(Vector3 position)
    {
        Vector3 bulletSpread = new(Random.Range(-stats.spread, stats.spread), 0 , Random.Range(-stats.spread, stats.spread));
        Vector3 direction = (position - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction + Vector3.up * Mathf.PI;
        rb.linearVelocity = direction * stats.speed + bulletSpread;
        
        Destroy(gameObject, stats.lifeTimer);
    }
    #endregion
}
