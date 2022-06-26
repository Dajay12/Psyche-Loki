using UnityEngine;

public class PlayerTopDown : MonoBehaviour
{
    /*
     Shit I need to add to the player Movement Scritp:
     1. Mobile controls (Completed)
    */

    [Header("Movement")]
    Rigidbody2D rb;
    Vector2 movement;
    public float mSpeed = 5;
    public Joystick joystick;

    private enum PlayerAction { Normal, Dodge }
    private PlayerAction playerAction;

    [SerializeField] private bool dodging;
    private Vector3 dodgeDir;
    private float dodgeSpd;
    [SerializeField] private Transform dodgePoint;

    private void Awake()
    {
        playerAction = PlayerAction.Normal;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        // Controls & Movement
        movement.x = joystick.Horizontal * mSpeed;
        movement.y = joystick.Vertical * mSpeed;
        rb.MovePosition(rb.position + movement * Time.deltaTime);

        if (joystick.Horizontal >= .1f)
        {
            movement.x = mSpeed;
            transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, 0));
        }
        else if (joystick.Horizontal <= -.1f)
        {
            movement.x = -mSpeed;
            transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, 180));
        }

        if (joystick.Vertical >= .1f)
        {
            movement.y = mSpeed;
            transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, 270));
        }
        else if (joystick.Vertical <= -.1f)
        {
            movement.y = -mSpeed;
            transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, 90));
        }

        if (joystick.Horizontal >= .1f && joystick.Vertical >= .1f)
        {
            transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, 315));
        }
        else if (joystick.Horizontal <= -.1f && joystick.Vertical >= .1f)
        {
            transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, 225));
        }
        else if (joystick.Horizontal <= -.1f && joystick.Vertical <= -.1f)
        {
            transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, 135));
        }
        else if (joystick.Horizontal >= .1f && joystick.Vertical <= -.1f)
        {
            transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, 45));
        }


        // Dodge
        if (dodging == true)
        {
            playerAction = PlayerAction.Dodge;
            dodgeSpd = 25f;
            //dodgeDir = dodgePoint.position;
            //dodgeDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            dodgeDir = (dodgePoint.position - transform.position).normalized;
            dodging = false;
        }

        if (playerAction == PlayerAction.Dodge)
        {
            transform.position += dodgeDir * dodgeSpd * Time.deltaTime;
            dodgeSpd -= dodgeSpd * 10f * Time.deltaTime;

            if (dodgeSpd < 5f)
            {
                playerAction = PlayerAction.Normal;
            }
        }
    }
    public void DodgeAtivate()
    {
        dodging = true;
    }

    #region PC control

    //old one
    //dashisngInput = Input.GetKeyDown(KeyCode.LeftShift);
    /*  if (direction == 0) {
         if (Input.GetKeyDown(KeyCode.D)) direction = 1;
         else if (Input.GetKeyDown(KeyCode.S)) direction = 2;
         else if (Input.GetKeyDown(KeyCode.A)) direction = 3;
         else if (Input.GetKeyDown(KeyCode.W)) direction = 4;

     }
      else {
         if (dashTime <= 0) {
             direction = 0;
             dashTime = startDashTime;
             rb.velocity = Vector2.zero;
         }
         else {
             dashTime -= Time.deltaTime;
             if (dashisngInput && direction == 1) rb.velocity = Vector2.right * dashSpeed;
             else if (dashisngInput && direction == 2) rb.velocity = Vector2.down * dashSpeed;
             else if (dashisngInput && direction == 3) rb.velocity = Vector2.left * dashSpeed;
             else if (dashisngInput && direction == 4) rb.velocity = Vector2.up * dashSpeed;
         }
     }*/
    #endregion

}