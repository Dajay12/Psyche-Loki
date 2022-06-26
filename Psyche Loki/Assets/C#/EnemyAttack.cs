using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Transform target;
    float speed;

    public int health = 3;

    [Header ("Enemy Combat")]
    public Transform attackPoint;
    [Range(0,1)]
    public float attackRange;
    public float attackForce = 10;
    public LayerMask playerLayers;
    private float rotateSpeed = 15f;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        speed = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //follow the player
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Attack();

        //Aiming
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        if (target == null)
            return;
    }
    
    void Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach(Collider2D player in hitPlayer)
        {
            print("Attack " + player.name);
            //playerDetect = true;
            Vector2 direction = player.transform.position - transform.position;
            player.GetComponent<Rigidbody2D>().AddForce(direction * attackForce);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.green;
    }

}
