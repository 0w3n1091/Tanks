using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform firePoint;
    public GameObject enemyBulletPrefab;
    public int maxShootingInterval;
    public int minShootingInterval;

    
    void Start()
    {
       StartCoroutine(ShootCoroutine()); 
    }

    /// <summary>
    /// Enemy attacks at random intervals between minShootingInterval and maxShootingInterval
    /// </summary>
    private IEnumerator ShootCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minShootingInterval, maxShootingInterval));
            GameObject enemyBullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            enemyBullet.GetComponent<BulletControl>().direction = GetComponent<EnemyControl>().turnDirection;
        }
    }
}
