using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public enum EnemyAction { Roam, Attacker }
    public EnemyAction enemyAction;

    Transform target;
    float speed;

    public int health = 3;

    [SerializeField] bool combatActivate;

    [SerializeField] Transform combatPoint;
    public LayerMask combatLayer;

    private float turnSpeed = 2f;
    private float fireRate = 2.5f;
    private float nextTimeToFire = 0;

    float directionTime;
    private float roamTime;
    int roamPattern;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.5f;
        roamTime = 3f;

        enemyAction = EnemyAction.Roam;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (enemyAction == EnemyAction.Attacker && Vector2.Distance(transform.position, target.position) < 10f) { 
            combatActivate = true; 
        }

        //Aiming
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);

        if (target == null)
            return;
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

            if (Vector2.Distance(transform.position, target.position) < 10f) enemyAction = EnemyAction.Attacker;
        }

        if (enemyAction == EnemyAction.Attacker)
        {
            if (Vector2.Distance(transform.position, target.position) > 10f) { 
                enemyAction = EnemyAction.Roam;
                combatActivate = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(combatPoint.position, .5f);
        Gizmos.color = Color.green;
    }

}
