using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Joystick joystick;

    private Rigidbody2D rb;

    private float leftForce,
        rightForce,
        upForce,
        downForce;

    private Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = new Vector2();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartUp();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            StopUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartDown();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            StopDown();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartLeft();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartRight();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopRight();
        }
    }

    private void FixedUpdate()
    {
        moveDirection.x = joystick.Horizontal * speed;
        moveDirection.y = joystick.Vertical * speed;
        if (moveDirection != Vector2.zero)
        {
            transform.up = moveDirection;
        }
        rb.MovePosition((Vector2)transform.position + moveDirection);
    }

    public void StartLeft()
    {
        leftForce = -speed;
    }

    public void StopLeft()
    {
        leftForce = 0;
    }

    public void StartRight()
    {
        rightForce = speed;
    }

    public void StopRight()
    {
        rightForce = 0;
    }

    public void StartUp()
    {
        upForce = speed;
    }

    public void StopUp()
    {
        upForce = 0;
    }

    public void StartDown()
    {
        downForce = -speed;
    }

    public void StopDown()
    {
        downForce = 0;
    }
}
