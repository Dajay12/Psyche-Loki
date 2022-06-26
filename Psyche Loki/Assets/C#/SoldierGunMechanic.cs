using UnityEngine;
public class SoldierGunMechanic : MonoBehaviour
{
    Transform target;
    private float speed = 5;

    public Transform firePoint;
    public GameObject bulletPrefab;
    float bulletForce;

    int shootTimer;

    [Range(1, 5)]
    public int sphereRange;

    void Start()
    {
        InvokeRepeating("Aim", 0f, .5f);
        bulletForce = 75 * Time.deltaTime * speed;
    }

    void Update()
    {
        //shoots eveytime the ShootTimer reaches 300
        shootTimer++;
        if (shootTimer >= 300)
        {
            Shoot();
            shootTimer = 0;
        }

        //Rotate the Gun to aim at enemies
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
    }

    void Aim()
    {
        //Aims at closest enemy near this gameObject
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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

        if (nearestEnemy != null && shortestDistance <= sphereRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        //Spawn in a bullet prefab and apply force once the gameObject is within the scene
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRange);
    }
}
