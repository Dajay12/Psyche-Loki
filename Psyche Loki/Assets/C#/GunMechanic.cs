using UnityEngine;

public class GunMechanic : MonoBehaviour
{
    public PlayerHealth refToHealth;
    GameObject Player;

    Transform target;
    private float speed = 45f;

    public Transform firePoint;
    public GameObject bulletPrefab;
    //float bulletForce;

    [Range(0, 6)]
    public int sphereRange;

    [SerializeField] bool tap2Aim;

    private float fireRate = 2.5f;
    private float nextTimeToFire = 0;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        refToHealth = Player.GetComponent<PlayerHealth>();
    }

    void Start()
    {
        InvokeRepeating("Aim", 0f, .5f);
        //bulletForce = 40 * Time.deltaTime * speed;
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);

            RaycastHit2D tap = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.zero), 10f);
            if(tap)
            {
                Debug.Log("You've tapped " + tap.transform.name);
            }



            //Debug.Log(touchPos);
            Debug.DrawLine(transform.position, touchPos, Color.green);
        }

        //Aiming
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

        if (target == null)
            return;

    }

    void Aim()
    {
        if (tap2Aim != true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        else if (tap2Aim == true)
        {
            Debug.Log("stop touch my face");
        }

    }

    public void Shoot()
    {
        if (Time.time >= nextTimeToFire)
        {
            //Greater the firerate fast the gun shoot and vice versa.
            nextTimeToFire = Time.time + 1f / fireRate;

            Debug.DrawRay(firePoint.transform.position, transform.TransformDirection(Vector2.right) * 6f, Color.red);
            if (refToHealth.currentHealth > 0)
            {
                //Shooting bullets GameObject 
                //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

                //Raycasting bullets
                RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, transform.TransformDirection(Vector2.right), 6f);
                if (hit)
                {
                    Debug.Log("hit " + hit.collider.name);

                    Enemy Target = hit.transform.GetComponent<Enemy>();
                    if (Target != null)
                    {
                        Target.TakeDamage(1);
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRange);
    }
}
