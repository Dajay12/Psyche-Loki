using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlerter : MonoBehaviour
{
    public enum EnemyAction { Alerter, Roam }
    public EnemyAction enemyAction;

    Transform target;

    private float turnSpeed = 1.4f;
    float stoppingDistance;
    float retreatDistance;
    public float speed;

    float directionTime;
    private float roamTime;
    int roamPattern;

    public int health = 3;

    public bool closeCombat;

    [SerializeField] Transform[] spawn;
    [SerializeField] private GameObject[] enemyDetected;
    [SerializeField] private List<GameObject> enemyVariant;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {
        speed = 2.5f;
        retreatDistance = 5f;
        stoppingDistance = 6.5f;

        roamTime = 3f;

        enemyAction = EnemyAction.Roam;
    }

    void Update()
    {
        enemyDetected = GameObject.FindGameObjectsWithTag("Enemy");

        Movement();
        if (enemyAction == EnemyAction.Roam && Vector2.Distance(transform.position, target.position) < 10f)
        {
            SpawnEnemies();
            
            //Aiming
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
    }
    public int numPattern;
    void SpawnEnemies()
    {
        GameObject spawnVariants = enemyVariant[Random.Range(0, enemyVariant.Count)];
        
         numPattern++;
         Debug.Log("num of enemy " + enemyDetected.Length);

        if (numPattern >= 60)
        {
            //Instantiate(spawnVariants, spawn[0].transform);
            Instantiate(enemyVariant[0], spawn[0].transform);
            numPattern = 0;
        }

        /*GameObject spawnVariants = enemyVariant[Random.Range(0, enemyVariant.Count + 1)];
        Instantiate(spawnVariants, spawn[0].transform);
        Instantiate(spawnVariants, spawn[1].transform);
        Instantiate(spawnVariants, spawn[2].transform);
        Instantiate(spawnVariants, spawn[3].transform);
        Instantiate(spawnVariants, spawn[4].transform);

        if(Input.GetKeyDown(KeyCode.Mouse0))Instantiate(spawnVariants, spawn[0].transform);*/
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

            if (Vector2.Distance(transform.position, target.position) < 10f) enemyAction = EnemyAction.Alerter;           
        }

        if (enemyAction == EnemyAction.Alerter)
        {

            if (Vector2.Distance(transform.position, target.position) < 4f) closeCombat = true;
            else if (Vector2.Distance(transform.position, target.position) > 10f) enemyAction = EnemyAction.Roam;
            else closeCombat = false;

            if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
            {
                transform.position = transform.position;
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

        //transform.position = transform.position;

        //if (Vector2.Distance(transform.position, target.position) > 10f) movement = EnemyMovement.Roam;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyAction == EnemyAction.Roam)
        {
            speed = speed * -1;
        }
    }
}
