using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float RotateForce = 0.5f;
    public float JumpForce = 0.5f;

    private Rigidbody2D rb = null;
    private float rotation = 0.0f;
    private bool onGround = false;
    private Vector2 jumpDirection = new(0.0f, 0.0f);
    private bool shouldJump = false;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        var clockwise = Input.GetKey(KeyCode.A) ? 1.0f : 0.0f;
        var counterClockwise = Input.GetKey(KeyCode.D) ? -1.0f : 0.0f;
        rotation = clockwise + counterClockwise;

        if (onGround && Input.GetKey(KeyCode.Space))
            shouldJump = true;
        onGround = false;

        var camera = Camera.main;

        camera.transform.position = new(transform.position.x, transform.position.y, -10.0f);

        GravityObject nearestObject = null;
        foreach (var obj in GravityObject.Objects)
        {
            if (obj.gameObject != gameObject)
            {
                if (nearestObject == null)
                {
                    nearestObject = obj;
                    continue;
                }

                var newGravity = obj.rb.mass / (obj.transform.position - transform.position).sqrMagnitude;
                var currentGravity = nearestObject.rb.mass / (nearestObject.transform.position - transform.position).sqrMagnitude;
                if (newGravity > currentGravity)
                    nearestObject = obj;
            }
        }

        if (nearestObject == null)
            return;

        var toNearest = nearestObject.transform.position - transform.position;
        var distance = toNearest.magnitude;
        camera.orthographicSize = distance * 2.0f;
        camera.transform.up = -toNearest;
    }

    public void FixedUpdate()
    {
        rb.AddTorque(rotation * RotateForce);

        if (shouldJump)
        {
            shouldJump = false;
            if (jumpDirection.sqrMagnitude > 0.0)
                rb.AddForce(jumpDirection.normalized * JumpForce, ForceMode2D.Impulse);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        onGround = true;
        jumpDirection = new(0.0f, 0.0f);
        for (var i = 0; i < collision.contactCount; i++)
        {
            var contact = collision.GetContact(i);
            if (contact.otherRigidbody == rb)
                jumpDirection += contact.normal;
            else
                jumpDirection -= contact.normal;
        }
    }
}
