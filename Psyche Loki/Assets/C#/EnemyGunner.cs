using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunner : MonoBehaviour
{
    public enum EnemyAction { Roam, Ranger}
    public EnemyAction enemyAction;

    Transform target;
    [SerializeField] Transform combatPoint;
    [SerializeField] GameObject bulletPrefab;
    public LayerMask combatLayer;
    
    public float speed;
    private float turnSpeed = 2f;
    private float fireRate = 2.5f;
    private float nextTimeToFire = 0;

    float stoppingDistance;
    float retreatDistance;

    float directionTime;
    private float roamTime;
    int roamPattern;

    public int health = 3;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {
        speed = 2.5f;
        retreatDistance = 3f;
        stoppingDistance = 4.5f;

        roamTime = 3f;

        enemyAction = EnemyAction.Roam;
    }

    void Update()
    {
        Movement();
        if (enemyAction == EnemyAction.Ranger && Vector2.Distance(transform.position, target.position) < 10f)
            Shoot();
    }

    void Shoot()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);

        if (target == null)
            return;

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 3f / fireRate;

            GameObject bullet = Instantiate(bulletPrefab, combatPoint.position, combatPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        }
    }

    void Movement()
    {
        if (enemyAction == EnemyAction.Roam)
        {
            directionTime += Time.deltaTime;
            if (roamPattern >= 3) roamPattern = 0;

            if (roamPattern == 0) transform.position += new Vector3(speed * Time.deltaTime, 0);
            else if (roamPattern == 1) transform.position += new Vector3(0, speed * Time.deltaTime);
            else if (roamPattern == 2) transform.position += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime);

            if (directionTime >= roamTime)
            {
                roamPattern++;
                speed = speed * -1;
                directionTime = 0;
            }

            if (Vector2.Distance(transform.position, target.position) < 10f) enemyAction = EnemyAction.Ranger;
        }

        if (enemyAction == EnemyAction.Ranger)
        {
            if (Vector2.Distance(transform.position, target.position) > 10f) enemyAction = EnemyAction.Roam;

            if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance)
            {
                transform.position = transform.position;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyAction == EnemyAction.Roam)
        {
            speed = speed * -1;
        }
    }

    void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, sphereRange);
        Gizmos.DrawWireCube(transform.position, new Vector3(cubeRange, cubeRange));*/
    }
}
