using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyMovement : MonoBehaviour
{
    public enum EnemyMovement { Idle, Attacker, Ranger }
    public EnemyMovement movement;

    public Transform travelPoint;
    Transform target;

    float stoppingDistance;
    float retreatDistance;
    float speed;

    /// <summary>
    /// 1.AttackerSpeed = fasted 
    /// 2.ShooterSpeed = balanced 
    /// 3.HealerSpeed = Slow 
    /// </summary>
    /// 
    private void Awake()
    {
        stoppingDistance = 3.5f;
        retreatDistance = 4f;
        speed = 2.5f;
        
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        /*//Aiming
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);*/

        if (Vector2.Distance(transform.position, target.position) > 10f) { 
            movement = EnemyMovement.Idle;
            transform.position = transform.position;
        }
        if (movement == EnemyMovement.Idle);

        switch (movement) {
            case EnemyMovement.Ranger:
                Range_Movement();
                break;
            case EnemyMovement.Attacker:
                Close_Movement();
                break;
            default:
                movement = EnemyMovement.Idle;
                break;
        }
    }

    void Close_Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void Range_Movement()
    {
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
    }
}
