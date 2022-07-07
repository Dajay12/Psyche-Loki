using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private float nextFireTime = 0;

    public PlayerTopDown refToPlayer;
    public PlayerHealth refToHealth;
    public Transform storage;

    [SerializeField] public enum AbilityUse { None, Invincibility, Restrain, BioExplosion }
    [SerializeField] public AbilityUse abilityUse;
    public bool ability;
    public bool swapped;
    public bool occupied;

    private void Awake()
    {
        abilityUse = AbilityUse.None;
    }

    void Start()
    {
        abNumTracker = Time.time;
    }

    void Update()
    {
        if (!swapped)
        {
            switch (abilityUse)
            {
                default:
                    abilityUse = AbilityUse.None;
                    break;
                case AbilityUse.Invincibility:
                    Invincibility();
                    num = invincibileTime;
                    break;
                case AbilityUse.BioExplosion:
                    Explosion();
                    num = explodeTime;
                    break;
                case AbilityUse.Restrain:
                    Restrain();
                    num = restrainTime;
                    break;
            }
        }
        AbilityTracker();
    }


    [Header("Invincibility")]
    public float invincibileTime = 10;
    void Invincibility()
    {
        if (abilityUse == AbilityUse.Invincibility)
        {
            if (Time.time > nextFireTime)
            {
                if (ability == true)
                {
                    trackerOn = true;
                    nextFireTime = Time.time + invincibileTime;

                    ability = false;
                    refToHealth.alive = false;

                    GetComponent<SpriteRenderer>().color = new Vector4(0, 1, 1, 1);
                }
                else
                {
                    refToHealth.alive = true;
                    GetComponent<SpriteRenderer>().color = new Vector4(0, 0, 1, 1);
                }
            }
        }

    }

    [Header("Restrain")]
    public float restrainTime = 10;
    void Restrain()
    {
        string tag = "Enemy";
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

        if (abilityUse == AbilityUse.Restrain)
        {
            if (Time.time > nextFireTime)
            {
                if (ability == true)
                {
                    trackerOn = true;
                    nextFireTime = Time.time + restrainTime;

                    foreach (GameObject tagged in taggedObjects)
                    {
                        //disable the enemies movement script
                        tagged.GetComponent<enemyMovement>().enabled = false;
                    }

                    ability = false;
                    GetComponent<SpriteRenderer>().color = new Vector4(1, 0, 1, 1);
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = new Vector4(0, 0, 1, 1);
                    foreach (GameObject tagged in taggedObjects)
                    {
                        //enable the enemies movement script
                        tagged.GetComponent<enemyMovement>().enabled = true;
                    }
                }
            }
        }
    }

    [Header("Explosion")]
    public float explodeTime;
    [Range(0, 6)]
    public float explosionRadius;
    public float explosiveForce;

    public LayerMask layerToHit;
    public bool explosiveActivate;
    void Explosion()
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerToHit);

        if (Time.time > nextFireTime)
        {
            if (ability == true)
            {
                trackerOn = true;
                nextFireTime = Time.time + explodeTime;

                foreach (Collider2D nearbyObjects in obstacles)
                {
                    Vector2 direction = nearbyObjects.transform.position - transform.position;
                    Rigidbody2D rb = nearbyObjects.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.AddForce(direction * explosiveForce);
                    }
                    Enemy explosiveDamage = nearbyObjects.GetComponent<Enemy>();
                    if (explosiveDamage != null)
                    {
                        explosiveDamage.health -= 2;
                    }
                }

                ability = false;
                GetComponent<SpriteRenderer>().color = new Vector4(1, 0, 0, 1);
            }
            else
            {
                explosiveActivate = false;
                GetComponent<SpriteRenderer>().color = new Vector4(0, 0, 1, 1);
            }
        }
    }

    public void AbilityAtivate()
    {
        if (!swapped)
        {
            if (abilityUse != AbilityUse.None)
            {
                ability = true;
                abNumTracker = 0;
            }
        }
        else if (swapped)
        {
            ability = true;
        }
    }

    public GameObject abilityStored;
    float abNumTracker, num;
    bool trackerOn;
    void AbilityTracker()
    {
        if (trackerOn)
        {
            abNumTracker += Time.deltaTime;
            if (abNumTracker > num + .1f)//10.9f)
            {
                trackerOn = false;
                occupied = false;

                abilityUse = AbilityUse.None;
                Destroy(abilityStored);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
