using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] Enemy typeOfEnemy;
    public enum EnemyMovement { Roam, Alert, Attacker, Ranger, Tank }
    public EnemyMovement movement;

    Transform target;

    float stoppingDistance;
    float retreatDistance;
    public float speed;

    float directionTime;
    public float roamTime;
    int roamPattern;

    public bool closeCombat;

    /// <summary>
    /// 1.AttackerSpeed = fasted 
    /// 2.ShooterSpeed = balanced 
    /// 3.HealerSpeed = Slow 
    /// </summary>
    /// 
    private void Awake()
    {
        stoppingDistance = 4.5f;
        retreatDistance = 3f;

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {

    }


    void Update()
    {
        /*if (Vector2.Distance(transform.position, target.position) > 10f)
        {
            movement = EnemyMovement.Alert;
            
        }*/

        switch (movement)
        {
            default:
                movement = EnemyMovement.Roam;
                if (movement == EnemyMovement.Roam)
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

                    if (Vector2.Distance(transform.position, target.position) < 10f)
                    {
                        if (typeOfEnemy.enemyType == Enemy.EnemyType.Attacker) movement = EnemyMovement.Attacker;
                        if (typeOfEnemy.enemyType == Enemy.EnemyType.Ranger) movement = EnemyMovement.Ranger;
                        if (typeOfEnemy.enemyType == Enemy.EnemyType.Tank) movement = EnemyMovement.Tank;
                        if (typeOfEnemy.enemyType == Enemy.EnemyType.Alerter) movement = EnemyMovement.Alert;
                    }
                }
                break;
            case EnemyMovement.Ranger:
                speed = 2.5f;
                retreatDistance = 3f;
                stoppingDistance = 4.5f;
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
                break;
            case EnemyMovement.Attacker:
                speed = 2.5f;
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                break;
            case EnemyMovement.Tank:
                speed = 2.5f;
                retreatDistance = 4f;
                stoppingDistance = 5.5f;

                if (Vector2.Distance(transform.position, target.position) < 10f) closeCombat = true;
                else closeCombat = false;

                if (closeCombat)
                {
                    //Attack Player
                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
                else if (!closeCombat)
                {
                    //Shoot Player
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
                break;
            case EnemyMovement.Alert:
                speed = 2.5f;
                retreatDistance = 5f;
                stoppingDistance = 6.5f;
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

                //transform.position = transform.position;

                if (Vector2.Distance(transform.position, target.position) > 10f) movement = EnemyMovement.Roam;
                break;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (movement == EnemyMovement.Roam)
        {
            speed = speed * -1;
        }
    }
}
