using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveForce = 10.0f;
    public float RotateForce = 0.5f;

    private Rigidbody2D rb = null;
    private Vector2 moveDirection = new(0.0f, 0.0f);
    private float rotation = 0.0f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        var up = Input.GetKey(KeyCode.W) ? 1.0f : 0.0f;
        var down = Input.GetKey(KeyCode.S) ? 1.0f : 0.0f;
        var left = Input.GetKey(KeyCode.A) ? 1.0f : 0.0f;
        var right = Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;
        moveDirection =
            up * Vector2.up +
            down * Vector2.down +
            left * Vector2.left +
            right * Vector2.right;
        if (moveDirection.sqrMagnitude > 0.0)
            moveDirection.Normalize();
        var clockwise = Input.GetKey(KeyCode.Q) ? 1.0f : 0.0f;
        var counterClockwise = Input.GetKey(KeyCode.E) ? 1.0f : 0.0f;
        rotation = clockwise * 1.0f + counterClockwise * -1.0f;
    }

    public void FixedUpdate()
    {
        rb.AddForce(moveDirection * MoveForce);
        rb.AddTorque(rotation * RotateForce);
    }

    public void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, moveDirection);
        Gizmos.color = oldColor;
    }
}
