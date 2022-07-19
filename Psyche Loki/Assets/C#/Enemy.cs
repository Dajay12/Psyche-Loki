using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public enemyMovement myMovement;

    public enum EnemyType { None, Alerter, Attacker, Ranger, Tank }
    public EnemyType enemyType;

    Transform target;
    public Transform combatPoint;
    public LayerMask combatLayer;
    [SerializeField] bool combatActivate;
    [SerializeField] GameObject bulletPrefab;

    private float turnSpeed = 1.4f;
    private float fireRate = 2.5f;
    private float nextTimeToFire = 0;

    public int health = 3;

    [SerializeField]private GameObject[] spawn;
    [SerializeField]private GameObject[] enemyDetected;
    [SerializeField]private List<GameObject> enemyVariant;
    private void Awake()
    {
        myMovement = GetComponent<enemyMovement>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        spawn = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    void Start()
    {

    }

    void Update()
    {
        //Aiming
        if (myMovement.movement != enemyMovement.EnemyMovement.Roam)
        {
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }

        switch (enemyType)
        {
            case EnemyType.Attacker:
                combatActivate = true;
                break;
            case EnemyType.Ranger:
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 3f / fireRate;

                    GameObject bullet = Instantiate(bulletPrefab, combatPoint.position, combatPoint.rotation);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
                }
                break;
            case EnemyType.Tank:
                if (myMovement.closeCombat) combatActivate = true;
                else if (!myMovement.closeCombat) {
                    if (Time.time >= nextTimeToFire)
                    {
                        nextTimeToFire = Time.time + 3f / fireRate;

                        GameObject bullet = Instantiate(bulletPrefab, combatPoint.position, combatPoint.rotation);
                        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                        //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
                    }
                }
                break;
            case EnemyType.Alerter:
                enemyDetected = GameObject.FindGameObjectsWithTag("Enemy");

                if (enemyDetected.Length <= 1)
                {
                    Debug.Log("Here");
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (combatActivate)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(combatPoint.position, .5f, combatLayer);
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1.5f / fireRate;

                foreach (Collider2D player in hit)
                {
                    player.GetComponent<PlayerHealth>().currentHealth -= 1;

                    Vector2 pDirection = player.transform.position - transform.position;
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.AddForce(pDirection * 5000);
                        
                    }
                }
            }
        }
    }

    private void SpawnEnemies()
    {
        GameObject spawnVariants = enemyVariant[Random.Range(0, enemyVariant.Count)];
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }
    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(combatPoint.position, .5f);
    }
}
