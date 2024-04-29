using UnityEngine;

public class SetupOrbit : MonoBehaviour
{
    public GravityObject ThingToOrbitAround = null;

    public void Start()
    {
        var toThing = (Vector2)(ThingToOrbitAround.transform.position - transform.position);
        var distance = toThing.magnitude;
        var direction = toThing / distance;
        direction = new(direction.y, -direction.x);

        var rb = GetComponent<Rigidbody2D>();

        var orbitVelocity = direction * Mathf.Sqrt(
            GravityObject.GravityStrength * ThingToOrbitAround.rb.mass * ThingToOrbitAround.rb.mass /
            (distance * (ThingToOrbitAround.rb.mass + rb.mass))
        );
        var otherOrbitVelocity = -direction * Mathf.Sqrt(
            GravityObject.GravityStrength * rb.mass * ThingToOrbitAround.rb.mass /
            (distance * (ThingToOrbitAround.rb.mass + rb.mass))
        );

        rb.velocity = orbitVelocity - otherOrbitVelocity;
    }
}
