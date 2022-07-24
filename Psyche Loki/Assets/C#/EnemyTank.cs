using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    public enum EnemyAction { Tank, Roam }
    public EnemyAction enemyAction;

    Transform target;
    float speed;

    [SerializeField] Transform combatPoint;
    [SerializeField] LayerMask combatLayer;

    [SerializeField] bool combatActivate;
    [SerializeField] GameObject bulletPrefab;

    private float turnSpeed = 1.4f;
    private float fireRate = 2.5f;
    private float nextTimeToFire = 0;

    float stoppingDistance;
    float retreatDistance;

    float directionTime;
    private float roamTime;
    int roamPattern;

    [SerializeField] bool closeCombat;

    public int health = 3;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.5f;
        retreatDistance = 3f;
        stoppingDistance = 4.5f;

        roamTime = 3f;

        enemyAction = EnemyAction.Roam;
    }

    // Update is called once per frame
    void Update()
    {
        
        Movement();
        if(enemyAction == EnemyAction.Tank && Vector2.Distance(transform.position, target.position) < 10f) 
            Combat();
    }

    private void FixedUpdate()
    {
        if (combatActivate)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(combatPoint.position, .5f, combatLayer);

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

    void Combat()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);

        if (target == null)
            return;

        if (closeCombat) combatActivate = true;
        else if (!closeCombat)
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 3f / fireRate;

                GameObject bullet = Instantiate(bulletPrefab, combatPoint.position, combatPoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            }
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

            if (Vector2.Distance(transform.position, target.position) < 10f) enemyAction = EnemyAction.Tank;
        }

        if (enemyAction == EnemyAction.Tank)
        {
            if (Vector2.Distance(transform.position, target.position) < 4f) closeCombat = true;
            else if (Vector2.Distance(transform.position, target.position) > 10f) enemyAction = EnemyAction.Roam;
            else closeCombat = false;

            if (closeCombat)
            {
                //Attack Player
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else if (!closeCombat)
            {
                retreatDistance = 5f;
                stoppingDistance = 5.5f;
                //Shoot Player
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
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
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

}
