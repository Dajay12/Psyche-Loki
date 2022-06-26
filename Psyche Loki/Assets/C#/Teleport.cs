using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("Teleportation")]
    public Transform nextPos;
    public GameObject Player;

    public Animator anim;

    void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine("ShadeProcess");
        }
    }

    IEnumerator ShadeProcess()
    {
        anim.Play("Shade_On");

        Player.GetComponent<PlayerTopDown>().enabled = false;
        
        yield return new WaitForSeconds(1f);

        Player.GetComponent<PlayerTopDown>().enabled = true;

        if (this.transform.position.x >= Player.transform.position.x)
        {
            Player.transform.position = new Vector2(nextPos.transform.position.x + 2f, nextPos.transform.position.y + Player.transform.position.y);
        }
        else if (this.transform.position.x <= Player.transform.position.x)
        {
            Player.transform.position = new Vector2(nextPos.transform.position.x - 2f, nextPos.transform.position.y + Player.transform.position.y);
        }

        anim.Play("Shade_Off");
    }
}
