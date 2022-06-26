using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class egm : MonoBehaviour
{
    Transform target;
    private float speed = 25f;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public int timer;
    //float bulletForce;

    [Range(0, 6)]
    public int sphereRange;

    void Start()
    {
        InvokeRepeating("Aim", 0f, .5f);
        //bulletForce = 40 * Time.deltaTime * speed;
    }

    void Aim()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= sphereRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        //Shoot Input
        //if (Input.GetKeyDown(KeyCode.Space)) { Shoot(); }

        timer++;
        if (timer == 60) {
            
            timer = 0;
        }
    }

    
}

