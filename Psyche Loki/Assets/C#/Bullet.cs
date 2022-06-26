using UnityEngine;
public class Bullet : MonoBehaviour
{
    void Update()
    {

        Destroy(gameObject, 3f);
        transform.Translate(5.5f * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(1);
            Destroy(gameObject);
        }

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
            Destroy(gameObject);
        }

        Destroy(gameObject);
        //Debug.Log(collision.gameObject);
    }
}
