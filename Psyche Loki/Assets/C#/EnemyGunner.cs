using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunner : MonoBehaviour
{
    [Header("Enemy Movement")]
    public float stoppingDistance;
    public float retreatDistance;
    float mSpeed;
    
    [Header("Enemy Combat")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    //float bulletForce;
    [Range(0, 35)]
    public float cubeRange;
    private float WeaponRotateSpeed = 15f;
    Transform target;
    private float timeBtwShots;
    public float startTimeBtwShots;
    bool targetConfirm;

    [Header("Enemy Health")]
    public int health = 3;

    void Start()
    {
        InvokeRepeating("Aim", 0f, .1f);
        //bulletForce = 40 * Time.deltaTime * speed;

        mSpeed = 1.5f;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }

    void Update()
    {
        //Shooting
        if(timeBtwShots <= 0) {
            if (targetConfirm == true) Shoot();
            timeBtwShots = startTimeBtwShots;
        }
        else {
            timeBtwShots -= Time.deltaTime;
        }
        

        //follow the player
        if(Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, mSpeed * Time.deltaTime);
        } else if(Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        } else if(Vector2.Distance(transform.position, target.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, -mSpeed * Time.deltaTime);
        }

        //Aiming
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, WeaponRotateSpeed * Time.deltaTime);

        if (target == null)
            return;
    }
    
    void Aim()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= cubeRange)
        {
            target = nearestEnemy.transform;
            targetConfirm = true;
        }
        else
        {
            target = null;
            targetConfirm = false;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, sphereRange);
        Gizmos.DrawWireCube(transform.position, new Vector3(cubeRange, cubeRange));
    }
}
