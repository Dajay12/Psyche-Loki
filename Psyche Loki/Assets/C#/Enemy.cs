using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enemyMovement myMovement;

    public enum EnemyType { None, Alerter, Attacker, Ranger, Tank }
    public EnemyType enemyType;

    Transform target;
    public Transform combatPoint;
    public LayerMask combatLayer;

    private float turnSpeed = 1.4f;
    private float fireRate = 2.5f;
    private float nextTimeToFire = 0;

    public int health = 3;

    private void Awake()
    {
        myMovement = GetComponent<enemyMovement>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {

    }

    void Update()
    {
        //Aiming
        if (myMovement.movement != enemyMovement.EnemyMovement.Roam) {
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }

        switch (enemyType)
        {
            case EnemyType.Attacker:
                /*Collider2D[] hit = Physics2D.OverlapCircleAll(combatPoint.position, .5f, combatLayer);
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 3f / fireRate;

                    foreach (Collider2D player in hit)
                    {
                        player.GetComponent<PlayerHealth>().currentHealth -= 1;
                        
                        Vector2 pDirection = player.transform.position - transform.position;
                        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                        if (rb != null) { 
                            rb.AddForce(pDirection * 100, ForceMode2D.Force);
                            Debug.Log("Here");
                        }
                    }
                }*/
                break;
            case EnemyType.Ranger:
                break;
            case EnemyType.Tank:
                break;
            case EnemyType.Alerter:
                break;
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(combatPoint.position, .5f, combatLayer);
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 3f / fireRate;

            foreach (Collider2D player in hit)
            {
                player.GetComponent<PlayerHealth>().currentHealth -= 1;

                Vector2 pDirection = player.transform.position - transform.position;
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(pDirection * 5000);
                    Debug.Log("Here");
                }
            }
        }
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
