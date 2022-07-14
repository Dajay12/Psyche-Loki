using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enemyMovement myMovement;

    public enum EnemyType { None, Alerter, Attacker, Ranger, Tank}
    public EnemyType enemyType;

    Transform target;
    public int health = 3;
    float speed;
    //public CircleCollider2D body;
    Sprite mt;
    Animator anim;
    
    enemyMaker Attacker = new enemyMaker();

    private void Awake()
    {
        myMovement = GetComponent<enemyMovement>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {
        speed = 1.5f;
    }

    void Update()
    {
        //Aiming
        /*Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);*/

        //myMovement.closeCombat = true;

        if (enemyType == EnemyType.Attacker)
        {
            //myMovement.movement = enemyMovement.EnemyMovement.Attacker;
        }
        else if (enemyType == EnemyType.Ranger)
        {
           // myMovement.movement = enemyMovement.EnemyMovement.Ranger;
        }

        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
