using UnityEditorInternal;
using UnityEngine;

public class SpawnerTower : Tower
{
    #region Variables
    [SerializeField] float spawnRadius;
    [SerializeField] Enemy[] enemiesToSpawn;
    
    float spawnTimer;
    bool canSpawn;
    #endregion

    #region Unity Functions
    #endregion

    #region Other Functions
    protected override void UseAbility()
    {
        if (canSpawn && playerInRange)
        {
            spawnTimer = stats.useRate;
            canSpawn = false;

            Vector3 spawnPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * spawnRadius + transform.position;
            Enemy enemy = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)];
            for (int i = 0; i < Random.Range(1, 5); i++)
            {
                Enemy enemyObj = Instantiate(enemy, enemyContainer.transform);
                enemyObj.rb.position = spawnPosition +
                    new Vector3(i * (i % 2 == 1 ? -1 : 1), 0, i * (i % 2 == 1 ? 1 : -1));
            }
        }
        else
        {
            spawnTimer -= Time.deltaTime;
            canSpawn = spawnTimer < 0;
        }
    }
    #endregion
}
