using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealer : MonoBehaviour
{
    Transform target;
    Transform avoid;
    private float WeaponRotateSpeed = 15f;

    [Header("Enemy Movement")]
    public float stoppingDistance;
    public float retreatDistance;
    float mSpeed;

    void Start()
    {
        mSpeed = 1.5f;
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //follow the player
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, mSpeed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }

        //Aiming
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, WeaponRotateSpeed * Time.deltaTime);

        if (target == null)
            return;
    }
}
