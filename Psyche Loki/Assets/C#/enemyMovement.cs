using UnityEngine;
using UnityEngine.Tilemaps;

public class enemyMovement : MonoBehaviour
{
    public enum EnemyMovement { Roam, Alert, Attacker, Ranger, Tank }
    public EnemyMovement movement;

    //public Transform roamPoint;
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
            
        }
        Debug.Log(Tilemap.CellSwizzle.XYZ.ToString());
        Debug.Log(myTiles.size.ToString());*/

        switch (movement)
        {
            default:
                movement = EnemyMovement.Roam;
                if (movement == EnemyMovement.Roam) {
                    directionTime += Time.deltaTime;
                    if (roamPattern >= 3) roamPattern = 0;
                    
                    if (roamPattern ==  0) transform.position += new Vector3(speed * Time.deltaTime, 0);
                    else if (roamPattern == 1) transform.position += new Vector3(0, speed * Time.deltaTime);
                    else if (roamPattern == 2) transform.position += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime);

                    if (directionTime >= roamTime) {
                        roamPattern++;
                        speed = speed * -1;
                        directionTime = 0;
                    }
                }
                break;
            case EnemyMovement.Ranger:
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
                    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
                }
                break;
            case EnemyMovement.Attacker:
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                break;
            case EnemyMovement.Tank:
                if (closeCombat) {

                } else if (!closeCombat) {

                }
                break;
            case EnemyMovement.Alert:
                transform.position = transform.position;
                break;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(movement == EnemyMovement.Roam)
        {
            speed = speed * -1;
        }
    }
}
