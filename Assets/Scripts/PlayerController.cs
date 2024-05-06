using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float RotateForce = 0.5f;
    public float JumpForce = 0.5f;

    private GravityObject go = null;
    private float rotation = 0.0f;
    private bool onGround = false;
    private Vector2 jumpDirection = new(0.0f, 0.0f);
    private bool shouldJump = false;

    public void Awake()
    {
        go = GetComponent<GravityObject>();
    }

    public void Update()
    {
        var clockwise = Input.GetKey(KeyCode.A) ? 1.0f : 0.0f;
        var counterClockwise = Input.GetKey(KeyCode.D) ? -1.0f : 0.0f;
        rotation = clockwise + counterClockwise;

        if (Input.GetKey(KeyCode.Space))
            shouldJump = true;

        var camera = Camera.main;

        var toStrongest = go.StrongestObject.transform.position - transform.position;
        toStrongest.z = 0.0f;

        var distance = toStrongest.magnitude;

        camera.orthographicSize =
            Mathf.MoveTowards(camera.orthographicSize, distance * 2.5f, 10.0f * Time.deltaTime);
        camera.transform.SetPositionAndRotation(
            new(transform.position.x, transform.position.y, -10.0f),
            Quaternion.RotateTowards(
                camera.transform.rotation,
                Quaternion.FromToRotation(Vector3.up, -toStrongest),
                180.0f * Time.deltaTime
            )
        );
    }

    public void FixedUpdate()
    {
        go.rb.AddTorque(rotation * RotateForce);

        if (shouldJump && onGround && jumpDirection.sqrMagnitude > 0.0)
            go.rb.AddForce(jumpDirection.normalized * JumpForce, ForceMode2D.Impulse);

        shouldJump = false;
        onGround = false;
        jumpDirection = new(0.0f, 0.0f);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        onGround = true;
        for (var i = 0; i < collision.contactCount; i++)
        {
            var contact = collision.GetContact(i);
            if (contact.otherRigidbody == go.rb)
                jumpDirection += contact.normal;
            else
                jumpDirection -= contact.normal;
        }
    }
}
